using System;
using System.Data.SQLite;
using System.IO;

namespace habit_tracker
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!File.Exists("habit_tracker.sqlite"))
            {
                // create db file
                SQLiteConnection.CreateFile("habit_tracker.sqlite");

                // create new db connection
                SQLiteConnection initConnection = new SQLiteConnection("Data Source=habit_tracker.sqlite;Version=3");
                initConnection.Open();

                // create columns in the new db
                string sql = "create table aoe2_tracker (id INTEGER PRIMARY KEY, civilization string, win text)";
                SQLiteCommand command = new SQLiteCommand(sql, initConnection);
                command.ExecuteNonQuery();
                /*
                SQLiteCommand dummyCommand = new(initConnection);
                
                dummyCommand.CommandText = "INSERT INTO aoe2_tracker (civilization, win) VALUES ('berbers', 'yes')";
                dummyCommand.ExecuteNonQuery();

                dummyCommand.CommandText = "SELECT * FROM aoe2_tracker";
                var hasRows = dummyCommand.ExecuteReader().HasRows;
                if(hasRows)
                {
                    Console.WriteLine("Has rows");
                }
                */

            } else
            {
                Console.WriteLine("existing db found");
            }

            string[] Civilizations = { "britons", "byzantines", "celts", "chinese", "franks", "goths", "japanese", "mongols", "persians",
                                       "saracens", "teutons", "turks", "vikings", "aztecs", "huns", "koreans", "mayans", "spanish", 
                                       "incas", "indians", "italians", "magyars", "slavs", "berbers", "ethiopians", "malians", "portuguese",
                                       "burmese", "khmer", "malay", "vietnamese", "bulgarians", "cumans", "lithuanians", "tatars", "burgundians",
                                       "sicilians", "bohemians", "poles" 
                                     };

            MainProgram();

            void MainProgram()
            {
                while (true)
                {

                    WriteMenu();

                    int userInput = GetInput(Console.ReadLine());
                    Console.WriteLine();

                    // determine correct response to validated input
                    switch (userInput)
                    {
                        case 1:
                            PrintDb();
                            break;
                        case 2:
                            InsertRecord();
                            break;
                        case 3:
                            UpdateRecord();
                            break;
                        case 4:
                            DeleteRecord();
                            break;
                        case 0:
                            Environment.Exit(-1);
                            return;
                    }

                }
            }
            

            // write the main menu
            void WriteMenu()
            {
                Console.WriteLine("Welcome to Habit Tracker V1.0 currently in Alpha. Enter the number of the option you would like to choose.");
                Console.WriteLine();
                Console.WriteLine("1 - view database");
                Console.WriteLine("2 - log a new game");
                Console.WriteLine("3 - update an existing game");
                Console.WriteLine("4 - delete a game");
                Console.WriteLine("0 - quit program");
                Console.WriteLine();
            }
           
            // get input from user and validate
            int GetInput(string inputRaw)
            {
                int inputInt;

                // validate input
                while (true)
                {
                    if (inputRaw.Length == 1 && int.TryParse(inputRaw, out inputInt))
                    {
                        if (inputInt >= 0 && inputInt < 5)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Not a valid input, please try again");
                            Console.WriteLine();
                            inputRaw = Console.ReadLine();
                            Console.WriteLine();
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Not a valid input, please try again");
                        Console.WriteLine();
                        inputRaw = Console.ReadLine();
                        Console.WriteLine();
                    }
                }
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine();
                return inputInt;
            }

            
            void PrintDb()
            {

                // create the connection
                SQLiteConnection dbConnection = new SQLiteConnection("Data Source=habit_tracker.sqlite;Version=3");
                dbConnection.Open();

                string sql = "SELECT * FROM aoe2_tracker";
                SQLiteCommand printCommand = new SQLiteCommand(sql, dbConnection) ;
                SQLiteDataReader reader = printCommand.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine("game id #" + reader["id"] + " civ: " + reader["civilization"] + ", win: " + reader["win"]);
                }

                dbConnection.Close();

                Console.WriteLine("-----------------------------------------");
                Console.WriteLine();
                Console.WriteLine();
            }

            void InsertRecord()
            {
                // create the connection in the global scope
                SQLiteConnection dbConnection = new SQLiteConnection("Data Source=habit_tracker.sqlite;Version=3");
                dbConnection.Open();
                Console.WriteLine("Enter the civilization you played");
                Console.WriteLine(".");
                Console.WriteLine(".");
                PrintCivilizations();
                Console.WriteLine(".");
                string civ = Console.ReadLine().ToLower();
                Console.WriteLine();
                Console.WriteLine("Win or lose?");
                string win = Console.ReadLine().ToLower();
                Console.WriteLine();

                while(true)
                {
                    if(Array.IndexOf(Civilizations, civ) != -1)
                    {
                        break;
                    } else
                    {
                        Console.WriteLine("Not a valid civilization, please try again.");
                        civ = Console.ReadLine().ToLower();
                        Console.WriteLine();
                    }
                }

                string sql = "INSERT INTO aoe2_tracker (civilization, win) VALUES ('" + civ + "', '" + win + "')";
                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();
                dbConnection.Close();

                Console.WriteLine("Game logged successfully");
                Console.WriteLine();
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine();
            }

            void UpdateRecord()
            {
                // create the connection in the global scope
                SQLiteConnection dbConnection = new SQLiteConnection("Data Source=habit_tracker.sqlite;Version=3");
                dbConnection.Open();

                string id;
                string civ;
                string win;

                Console.WriteLine(".");
                Console.WriteLine(".");
                Console.WriteLine("List of Civs:");
                PrintCivilizations();
                Console.WriteLine(".");

                while (true)
                {
                    Console.WriteLine("Enter the number of the game would you like to update.");
                    id = Console.ReadLine();
                    Console.WriteLine();

                    if(int.TryParse(id, out int j))
                    {
                        SQLiteCommand cmd = new(dbConnection);
                        cmd.CommandText = "SELECT count(*) FROM aoe2_tracker WHERE id=" + id;
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if(count == 0)
                        {
                            Console.WriteLine("No such record exists. Please try again");
                        }
                        else
                        {
                            break;
                        }
                    }

                }
                
                while(true)
                {
                    Console.WriteLine("Enter the civ name you played as.");
                    civ = Console.ReadLine();
                    Console.WriteLine();

                    if(Array.IndexOf(Civilizations, civ) != -1)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("That civ does not exist in the game at this time. Please try again.");
                    }
                }
                

                Console.WriteLine("Win or Lose?");
                win = Console.ReadLine();
                Console.WriteLine();

                SQLiteCommand query = new SQLiteCommand("UPDATE aoe2_tracker SET civilization='"+civ+"', win='"+win+"' WHERE id='"+id+"'", dbConnection);
                query.ExecuteNonQuery();
                dbConnection.Close();

                Console.WriteLine("Game updated successfully");
                Console.WriteLine();
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine();
            }

            void DeleteRecord()
            {

                // create the connection in the current scope
                SQLiteConnection dbConnection = new SQLiteConnection("Data Source=habit_tracker.sqlite;Version=3");
                dbConnection.Open();

                Console.WriteLine("Enter the number of the game you would like to delete.");
                string input;
                int inputInt;

                // check if input is a number
                while (true)
                {
                    input = Console.ReadLine();
                    Console.WriteLine();

                    if(!int.TryParse(input, out inputInt))
                    {
                        Console.WriteLine("That is not a number, please try again.");
                    }
                    else
                    {
                        SQLiteCommand cmd = new(dbConnection);
                        cmd.CommandText = "SELECT count(*) FROM aoe2_tracker WHERE id=" + inputInt;
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count == 0)
                        {
                            Console.WriteLine("No such record exists. Please try again");
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                // now do the deletion
                string sql = "DELETE FROM aoe2_tracker WHERE id = '" + input + "'";

                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();
                dbConnection.Close();

                Console.WriteLine("Game deleted successfully");
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine();

            }

            void PrintCivilizations()
            {
                Console.WriteLine("List of possible civilizations");
                for( int i=0; i<Civilizations.Length; i++ )
                {
                    if(i+1 > Civilizations.Length)
                    {
                        Console.Write(Civilizations[i]);
                    }
                    else
                    {
                        Console.Write(Civilizations[i] + ", ");

                    }
                }
            }
        }
    }
}
