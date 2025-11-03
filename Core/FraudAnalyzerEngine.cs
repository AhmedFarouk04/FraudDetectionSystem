using System;
using System.Linq;
using Spectre.Console;
using Spectre.Console.Rendering;
using FraudDetectionSystem.Entities;
using FraudDetectionSystem.Services;
using FraudDetectionSystem.Extensions;

namespace FraudDetectionSystem.Core
{
    public static class FraudAnalyzerEngine
    {
        private static double _lastRiskScore = 0; 

        public static void Run(User user, RiskAnalyzer analyzer, AlertService alertService, TransactionService transactionService)
        {
            AnsiConsole.MarkupLine("\n[bold yellow]🔍 Starting Transaction Analysis...[/]");
            AnsiConsole.Progress()
                .Columns(new ProgressColumn[]
                {
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new SpinnerColumn(),
                })
                .Start(ctx =>
                {
                    var task = ctx.AddTask("[green]Analyzing Transactions[/]", maxValue: user.Transactions.Count);
                    foreach (var tx in user.Transactions)
                    {
                        analyzer.Analyze(tx);
                        task.Increment(1);
                        System.Threading.Thread.Sleep(400);
                    }
                });

            ShowDashboard(analyzer, alertService, transactionService);
        }

        public static void ShowDashboard(RiskAnalyzer analyzer, AlertService alertService, TransactionService transactionService)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Fraud Monitor").Color(Color.Aqua));
            AnsiConsole.Write(new Rule("[bold yellow]System Summary[/]"));

            var ruleTable = new Table().AddColumn("[yellow]Active Fraud Rules[/]");
            foreach (var rule in analyzer.GetActiveRules())
                ruleTable.AddRow($"[green]{rule.Name}[/]");
            AnsiConsole.Write(ruleTable);

            var alerts = alertService.GetAll().ToList();
            if (alerts.Any())
            {
                var alertTable = new Table()
                    .AddColumn("[red]Time[/]")
                    .AddColumn("[red]Severity[/]")
                    .AddColumn("[red]Message[/]")
                    .AddColumn("[red]Tx ID[/]");

                foreach (var alert in alerts)
                    alertTable.AddRow(alert.Timestamp.ToString("g"), alert.Severity, alert.Message, alert.Transaction.Id.ToString());

                var panel = new Panel(alertTable)
                    .Header($"Alerts ({alerts.Count})")
                    .BorderColor(Color.Red);
                AnsiConsole.Write(panel);
            }
            else
            {
                AnsiConsole.MarkupLine("[green]No active alerts. System is clean.[/]");
            }

            var total = transactionService.GetAll().Count();
            var highAlerts = alerts.Count(a => a.Severity == "High");
            var cleanTransactions = transactionService.GetAll().Count(tx => !alerts.Any(a => a.Transaction.Id == tx.Id));

            AnsiConsole.MarkupLine("\n[bold yellow]Summary[/]");
            AnsiConsole.MarkupLine($"[cyan]Total Transactions:[/] {total}");
            AnsiConsole.MarkupLine($"[red]High Severity Alerts:[/] {highAlerts}");
            AnsiConsole.MarkupLine($"[green]Clean Transactions:[/] {cleanTransactions}");

            var user = transactionService.GetAll().FirstOrDefault()?.User ?? new User(0, "Unknown", "N/A");
            ShowUserRiskBar(user, analyzer, transactionService);
        }

     
        public static void ShowUserRiskBar(User user, RiskAnalyzer analyzer, TransactionService transactionService)
        {
            var riskService = new RiskScoreService();
            var transactions = transactionService.GetAll().ToList();

            if (transactions.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No transactions yet to calculate risk.[/]");
                return;
            }

            double totalScore = 0;
            int count = 0;
            foreach (var tx in transactions)
            {
                var triggeredRules = analyzer.GetActiveRules().Where(r => r.IsSuspicious(tx)).ToList();
                totalScore += riskService.CalculateScore(tx, triggeredRules);
                count++;
            }

            var avgRisk = count > 0 ? totalScore / count : 0;
            var riskText = riskService.InterpretScore(avgRisk);

            var color = avgRisk switch
            {
                < 30 => Color.Green,
                < 70 => Color.Yellow,
                _ => Color.Red
            };

            AnsiConsole.Write(new Rule("[bold yellow]User Risk Profile[/]"));

            AnsiConsole.Write(
                new BarChart()
                    .Width(60)
                    .Label("[bold]Average Risk[/]")
                    .CenterLabel()
                    .AddItem($"{user.Name}", Math.Round(avgRisk, 1), color)
            );

            AnsiConsole.MarkupLine($"\nOverall Risk Level: [bold yellow]{avgRisk:F1}%[/] ([white]{riskText}[/])");

            ShowUserRiskGauge(avgRisk);

            double previousRisk = _lastRiskScore;
            _lastRiskScore = avgRisk;

            string trendIcon = avgRisk > previousRisk ? "↑" : avgRisk < previousRisk ? "↓" : "→";
            string trendColor = avgRisk > previousRisk ? "red" : avgRisk < previousRisk ? "green" : "grey";
            string trendText = avgRisk > previousRisk ? "Risk Increasing" : avgRisk < previousRisk ? "Risk Decreasing" : "Stable";
            AnsiConsole.MarkupLine($"[bold {trendColor}]Trend: {trendIcon} {trendText}[/]");

            AnsiConsole.Write(new Rule("[gray]End of Risk Summary[/]"));
        }

      
        public static void ShowUserRiskGauge(double avgRisk)
        {
            var color = avgRisk switch
            {
                < 30 => Color.Green,
                < 70 => Color.Yellow,
                _ => Color.Red
            };

            var gauge = new Panel(BuildGauge(avgRisk))
                .Header("[bold yellow]User Risk Gauge[/]")
                .BorderColor(color)
                .RoundedBorder();

            AnsiConsole.Write(gauge);
        }

        private static string BuildGauge(double value)
        {
            int filled = (int)Math.Round(value / 5);
            string gauge = "";

            for (int i = 0; i < 20; i++)
            {
                if (i < filled)
                {
                    gauge += i < 6 ? "[green]█[/]" : i < 13 ? "[yellow]█[/]" : "[red]█[/]";
                }
                else
                {
                    gauge += "[grey]░[/]";
                }
            }

            var label = value switch
            {
                < 30 => "[green]LOW[/]",
                < 70 => "[yellow]MEDIUM[/]",
                _ => "[red]HIGH[/]"
            };

            return $"{gauge}\n[bold white]{value:F1}%[/]  {label}";
        }

        public static void ShowDashboardForAll(
    List<User> users,
    RiskAnalyzer analyzer,
    AlertService alertService,
    TransactionService transactionService)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Multi-User Dashboard").Color(Color.Aqua));
            AnsiConsole.Write(new Rule("[bold yellow]Multi-User Risk Overview[/]"));

            var riskService = new RiskScoreService();
            var dataStorage = new DataStorageService("data.json"); 

            var chart = new BarChart()
                .Width(60)
                .Label("[bold]Average Risk per User[/]")
                .CenterLabel();

            foreach (var user in users)
            {
                double avgRisk = 0;

                if (user.Transactions.Any())
                {
                    double total = 0;
                    int count = 0;

                    foreach (var tx in user.Transactions)
                    {
                        double score = tx.RiskScore;

                        if (score <= 0)
                        {
                            var triggered = analyzer.GetActiveRules()
                                .Where(r => r.IsSuspicious(tx))
                                .ToList();

                            score = riskService.CalculateScore(tx, triggered);
                            tx.RiskScore = score; 
                        }

                        total += score;
                        count++;
                    }

                    avgRisk = count > 0 ? total / count : 0;
                }

                var color = avgRisk switch
                {
                    < 30 => Color.Green,
                    < 70 => Color.Yellow,
                    _ => Color.Red
                };

                chart.AddItem($"{user.Name} ({user.Country})", avgRisk, color);
            }

            dataStorage.SaveData(users);

            AnsiConsole.Write(chart);
            AnsiConsole.MarkupLine("\n[gray]End of Multi-User Dashboard[/]");
        }



    }
}
