using FraudDetectionSystem.Entities;
using FraudDetectionSystem.Extensions;
using FraudDetectionSystem.Services;
using Spectre.Console;

namespace FraudDetectionSystem.Core
{
    public static class SimulationMenu
    {
        public static void Start(
            List<User> users,
            RiskAnalyzer analyzer,
            TransactionService transactionService,
            AlertService alertService)
        {
            var dataStorage = new DataStorageService("data.json");

            if (users.Count == 0)
            {
                AnsiConsole.Clear();

                var panel = new Panel(
                    new FigletText("Fraud Detection System")
                        .Centered()
                        .Color(Color.Aqua))
                    .Header("[bold yellow] System Initialization[/]")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Yellow)
                    .Padding(1, 1);

                AnsiConsole.Write(panel);
                AnsiConsole.MarkupLine("[gray]No users found. Let's create your first user to begin simulation.[/]\n");

                string name;
                string country;

                while (true)
                {
                    name = AnsiConsole.Ask<string>("[bold cyan]Enter user name:[/] ").Trim();

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        AnsiConsole.MarkupLine("[red] Name cannot be empty![/]");
                        continue;
                    }
                    if (name.Length < 3)
                    {
                        AnsiConsole.MarkupLine("[yellow] Name must be at least 3 characters.[/]");
                        continue;
                    }
                    if (name.Any(char.IsDigit))
                    {
                        AnsiConsole.MarkupLine("[red] Name cannot contain numbers![/]");
                        continue;
                    }
                    break;
                }

                while (true)
                {
                    country = AnsiConsole.Ask<string>("[bold cyan]Enter user country:[/] ").Trim();

                    if (string.IsNullOrWhiteSpace(country))
                    {
                        AnsiConsole.MarkupLine("[red] Country cannot be empty![/]");
                        continue;
                    }
                    if (country.Length < 3)
                    {
                        AnsiConsole.MarkupLine("[yellow] Country name seems too short![/]");
                        continue;
                    }
                    if (country.Any(char.IsDigit))
                    {
                        AnsiConsole.MarkupLine("[red] Country cannot contain numbers![/]");
                        continue;
                    }
                    break;
                }

                var newUser = new User(1, name, country);
                users.Add(newUser);

                dataStorage.SaveData(users);

                var successPanel = new Panel($"[green] User [bold]{name}[/] from [bold]{country}[/] created successfully![/]")
                    .Border(BoxBorder.Ascii)
                    .BorderColor(Color.Green)
                    .Padding(1, 1);
                AnsiConsole.Write(successPanel);

                AnsiConsole.MarkupLine("[gray]You can now start adding transactions and analyzing risk.[/]");
                AnsiConsole.MarkupLine("[yellow]Press any key to continue...[/]");
                Console.ReadKey(true);
            }

            var currentUser = users.Last();
            bool running = true;

            while (running)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(
                    new FigletText("Fraud Monitor")
                        .LeftJustified()
                        .Color(Color.Aqua));

                AnsiConsole.MarkupLine($"[bold yellow]Current User:[/] [cyan]{currentUser.Name}[/] ({currentUser.Country})");
                AnsiConsole.MarkupLine("\n[bold yellow]Choose an option:[/]");
                AnsiConsole.MarkupLine("1️⃣  Add new user");
                AnsiConsole.MarkupLine("2️⃣  Switch user");
                AnsiConsole.MarkupLine("3️⃣  Add transaction");
                AnsiConsole.MarkupLine("4️⃣  View dashboard");
                AnsiConsole.MarkupLine("5️⃣  Exit simulation");

                var choice = AnsiConsole.Ask<string>("\nEnter choice (1-5):");

                switch (choice)
                {
                    case "1":
                        {
                            string name;
                            string country;

                            while (true)
                            {
                                name = AnsiConsole.Ask<string>("Enter new user name:").Trim();

                                if (string.IsNullOrWhiteSpace(name))
                                {
                                    AnsiConsole.MarkupLine("[red]Name cannot be empty![/]");
                                    continue;
                                }

                                if (name.Any(char.IsDigit))
                                {
                                    AnsiConsole.MarkupLine("[red]Name cannot contain numbers![/]");
                                    continue;
                                }

                                if (users.Any(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                                {
                                    AnsiConsole.MarkupLine("[red]This username already exists! Please choose another.[/]");
                                    continue;
                                }

                                break;
                            }

                            while (true)
                            {
                                country = AnsiConsole.Ask<string>("Enter user country:").Trim();

                                if (string.IsNullOrWhiteSpace(country))
                                {
                                    AnsiConsole.MarkupLine("[red]Country cannot be empty![/]");
                                    continue;
                                }

                                if (country.Any(char.IsDigit))
                                {
                                    AnsiConsole.MarkupLine("[red]Country name cannot contain numbers![/]");
                                    continue;
                                }

                                break;
                            }

                            var newUser = new User(users.Count + 1, name, country);
                            users.Add(newUser);

                            dataStorage.SaveData(users);
                            AnsiConsole.MarkupLine($"[green]User {name} ({country}) added successfully![/]");
                        }
                        break;

                    case "2":
                        {
                            currentUser = SwitchUser(users);
                            dataStorage.SaveData(users);
                        }
                        break;

                    case "3":
                        {
                            AddTransaction(currentUser, analyzer, transactionService);
                            dataStorage.SaveData(users);
                        }
                        break;

                    case "4":
                        {
                            FraudAnalyzerEngine.ShowDashboardForAll(users, analyzer, alertService, transactionService);
                        }
                        break;

                    case "5":
                        running = false;
                        dataStorage.SaveData(users);
                        break;

                    default:
                        AnsiConsole.MarkupLine("[red]Invalid choice! Please select 1-5.[/]");
                        break;
                }

                if (running)
                {
                    AnsiConsole.MarkupLine("\nPress any key to continue...");
                    Console.ReadKey(true);
                }
            }
        }

        private static void AddUser(List<User> users)
        {
            AnsiConsole.MarkupLine("\n[bold yellow]Enter new user details:[/]");
            var name = AnsiConsole.Ask<string>("Name:");
            var country = AnsiConsole.Ask<string>("Country:");
            var newUser = new User(users.Count + 1, name, country);
            users.Add(newUser);
            AnsiConsole.MarkupLine($"[green]User {name} added successfully![/]");
        }

        private static User SwitchUser(List<User> users)
        {
            if (users.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No users found. Add one first![/]");
                return new User(0, "Unknown", "N/A");
            }

            var names = users.Select(u => $"{u.Name} ({u.Country})").ToList();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select user:[/]")
                    .AddChoices(names)
            );

            var selected = users.First(u => $"{u.Name} ({u.Country})" == choice);
            AnsiConsole.MarkupLine($"[cyan]Switched to user:[/] [bold]{selected.Name}[/]");
            return selected;
        }

        private static void AddTransaction(User user, RiskAnalyzer analyzer, TransactionService service)
        {
            AnsiConsole.MarkupLine($"\n[bold yellow]Enter transaction for {user.Name} ({user.Country}):[/]");

            decimal amount = 0;
            while (true)
            {
                var input = AnsiConsole.Ask<string>("Amount: ");
                if (decimal.TryParse(input, out amount) && amount > 0)
                    break;
                AnsiConsole.MarkupLine("[red]Invalid input. Please enter a valid number.[/]");
            }

            var country = AnsiConsole.Ask<string>("Country: ");

            var tx = new Transaction(
                id: service.GetAll().Count() + 1,
                user: user,
                amount: amount,
                country: country,
                time: DateTime.Now
            );

            user.Transactions.Add(tx);
            service.Add(tx);

            AnsiConsole.MarkupLine("[blue]Analyzing transaction...[/]");
            var result = analyzer.Analyze(tx);

            var triggeredRules = analyzer.GetActiveRules()
                .Where(r => r.IsSuspicious(tx))
                .Select(r => r.Name)
                .ToList();

            bool isSuspicious = triggeredRules.Any();

            var riskService = new RiskScoreService();
            var score = riskService.CalculateScore(tx, analyzer.GetActiveRules().Where(r => r.IsSuspicious(tx)).ToList());
            tx.RiskScore = score;
            var riskText = riskService.InterpretScore(score);

            var riskColor = score switch
            {
                < 30 => "green",
                < 70 => "yellow",
                _ => "red"
            };

            int filled = (int)(score / 5);
            string bar = new string('█', filled) + new string('░', 20 - filled);

            AnsiConsole.Write(new Rule("[cyan]Risk Analysis[/]"));
            AnsiConsole.MarkupLine($"[{riskColor}]{bar}[/]  [bold white]{score}%[/]  [bold {riskColor}]{riskText.ToUpper()}[/]");
            AnsiConsole.Write(new Rule("[gray]End of transaction analysis[/]"));

            var panelContent = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[yellow]Field[/]")
                .AddColumn("[white]Value[/]");

            panelContent.AddRow("User", $"{user.Name} ({user.Country})");
            panelContent.AddRow("Amount", $"{amount:C}");
            panelContent.AddRow("Country", $"{country}");
            panelContent.AddRow("Status", isSuspicious ? "[red]Suspicious Transaction Detected![/]" : "[green]Clean Transaction[/]");
            panelContent.AddRow("Triggered Rules", isSuspicious ? string.Join(", ", triggeredRules) : "None");
            panelContent.AddRow("Risk Score", $"[{riskColor}]{score:F1}% ({riskText.ToUpper()})[/]");

            var summaryPanel = new Panel(panelContent)
                .Header("[bold cyan]Transaction Summary[/]")
                .BorderColor(Color.Aqua)
                .RoundedBorder();

            AnsiConsole.Write(summaryPanel);


        }

    }
}
