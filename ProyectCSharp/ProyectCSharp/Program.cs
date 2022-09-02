using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ProyectCSharp
{
    class Program
    {
        private static List<TeamProyect> proyectsTeamsList;
        private static string fileNameOrigin;
        private static string fileNameDestiny;
        private static string pathString;
        private static string pathNameFileOrigin;
        private static string pathNameFileDestiny;
        private static string text;
        const decimal salaryPerDay = 45;

        static void Main(string[] args)
        {
            int option;
            fileNameOrigin = "MyFile.txt";
            fileNameDestiny = "ITCompanyData.txt";
            pathString = Directory.GetCurrentDirectory();
            pathNameFileOrigin = System.IO.Path.Combine(pathString, fileNameOrigin);
            pathNameFileDestiny = System.IO.Path.Combine(pathString, fileNameDestiny);

            do
            {
                Console.WriteLine("Choose and option:");
                Console.WriteLine("1.Load File");
                Console.WriteLine("2.Update Data");
                Console.WriteLine("3.Save Data");
                Console.WriteLine("4.Show Data");
                Console.WriteLine("5.Exit");
                option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        FirstLoadFile();
                        break;

                    case 2:
                        UpdateData();
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case 3:
                        SaveData();
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case 4:
                        ShowData();
                        Console.ReadLine();
                        Console.Clear();
                        break;

                    case 5:
                        Console.WriteLine("Exit");
                        break;
                    default:
                        break;
                }
            } while (option != 5);
        }

        static void FirstLoadFile()
        {
            try
            {
                LoadFileDestiny();
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("The file doesn't exist yet. The file will be created. Press enter to continue...");
                Console.ReadLine();
                CreateFile();
                LoadFileOrigin();
                WriteFile();
            }
            finally
            {
                Console.Clear();
                Console.WriteLine("The file is complete loaded. Press enter to choose another option...");
                Console.ReadLine();
            }
        }

        static void LoadFileOrigin()
        {
            if (System.IO.File.Exists(pathNameFileOrigin))
            {
                text = System.IO.File.ReadAllText(pathNameFileOrigin);
            }
            else
            {
                throw new Exception();
            }
        }

        static void LoadFileDestiny()
        {
            if (System.IO.File.Exists(pathNameFileDestiny))
            {
                text = System.IO.File.ReadAllText(pathNameFileDestiny);
            }
            else
            {
                throw new Exception();
            }
        }

        static void CreateFile()
        {
            if (!System.IO.File.Exists(pathNameFileDestiny))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(pathNameFileDestiny))
                {
                }
                Console.Clear();
                Console.WriteLine("File created correctly. Press enter to choose another option...");

            }
            else
            {
                Console.Clear();
                Console.WriteLine("File \"{0}\" already exists. Press enter to choose another option...", fileNameDestiny);
                return;
            }
        }

        static async Task WriteFile()
        {
            using StreamWriter file = new(pathNameFileDestiny, append: true);
            await file.WriteLineAsync(text);
            Console.WriteLine("File written correctly. Press enter to choose another option...");
        }

        static void UpdateData()
        {            
            try
            {
                LoadFileDestiny();
                proyectsTeamsList = JsonConvert.DeserializeObject<List<TeamProyect>>(text);

                foreach (var teamProyect in proyectsTeamsList)
                {
                    foreach (var programmer in teamProyect.programmers)
                    {
                        programmer.dateEnd = programmer.dateEnd.AddDays(1);
                    }
                }

                Console.Clear();
                Console.WriteLine("The date end of programmers has been updated. Press enter and then 3 to save the new data...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            
        }

        static void SaveData()
        {
            string json = JsonConvert.SerializeObject(proyectsTeamsList);
            File.WriteAllText(pathNameFileDestiny, json);

            Console.Clear();
            Console.WriteLine("The new date end was saved in file. Press enter to choose another option...");
        }

        static void ShowData()
        {            
            try
            {
                LoadFileDestiny();
                proyectsTeamsList = JsonConvert.DeserializeObject<List<TeamProyect>>(text);

                int nProgrammers = 0;
                int nProgrammerConsummed = 0;
                int daysConsummed = 0;
                int daysStillCharge = 0;
                int daysProgrammerMonth = 0;

                DateTime dateToday = DateTime.Now;
                DateTime dateStartMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var lastDayMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                DateTime dateEndMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, lastDayMonth);

                //Days consummed:
                foreach (var teamProyect in proyectsTeamsList)
                {
                    nProgrammers += teamProyect.programmers.Count();

                    foreach (var programmer in teamProyect.programmers)
                    {
                        if (programmer.dateEnd < dateStartMonth)
                        {
                            //No days has been consummed this month.
                        }
                        else if (programmer.dateStart < dateStartMonth && programmer.dateEnd >= dateStartMonth)
                        {
                            daysConsummed = daysConsummed + (programmer.dateEnd - dateStartMonth).Days;
                            nProgrammerConsummed++;
                        }
                        else if (programmer.dateStart >= dateStartMonth && programmer.dateEnd <= dateEndMonth)
                        {
                            daysConsummed = daysConsummed + (programmer.dateEnd - programmer.dateStart).Days;
                            nProgrammerConsummed++;
                        }
                        else if (programmer.dateStart >= dateStartMonth && programmer.dateEnd > dateEndMonth)
                        {
                            daysConsummed = daysConsummed + (dateEndMonth - programmer.dateStart).Days;
                            daysStillCharge = daysStillCharge + (programmer.dateEnd - dateEndMonth).Days;
                            nProgrammerConsummed++;
                        }
                        else if (programmer.dateStart > dateEndMonth)
                        {
                            daysStillCharge = daysStillCharge + (programmer.dateEnd - programmer.dateStart).Days;
                        }
                    }
                }//End days consummed.

                Console.WriteLine("IT COMPANY - Report:");
                Console.WriteLine();
                Console.WriteLine($"IT Company is actually composed of {proyectsTeamsList.Count} Project Teams and {nProgrammers} " +
                    $"Programmers.");
                Console.WriteLine();
                Console.WriteLine($"This month {daysConsummed} days have been consummed by {nProgrammerConsummed} programmers and " +
                    $"{daysStillCharge} days still in charge.");
                Console.WriteLine();
                Console.WriteLine("PROJECT TEAMS DETAILS:");
                Console.WriteLine();

                foreach (var teamProyect in proyectsTeamsList)
                {
                    Console.WriteLine($"Project team - {teamProyect.name}:");

                    foreach (var programmer in teamProyect.programmers)
                    {
                        if (programmer.dateStart < dateStartMonth && programmer.dateEnd >= dateStartMonth)
                        {
                            daysProgrammerMonth = (programmer.dateEnd - dateStartMonth).Days;
                        }
                        else if (programmer.dateStart >= dateStartMonth && programmer.dateEnd <= dateEndMonth)
                        {
                            daysProgrammerMonth = (programmer.dateEnd - programmer.dateStart).Days;
                        }
                        else if (programmer.dateStart >= dateStartMonth && programmer.dateEnd > dateEndMonth)
                        {
                            daysProgrammerMonth = (dateEndMonth - programmer.dateStart).Days;
                        }

                        Console.WriteLine($"- {programmer.lastName} {programmer.firstName}, in charge of {programmer.activity} " +
                            $"from {programmer.dateStart.ToString("dd-M-yyyy")} to {programmer.dateEnd.ToString("dd-M-yyyy")} " +
                            $"(duration: {(programmer.dateEnd - programmer.dateStart).Days}), this month: {daysProgrammerMonth} " +
                            $"days (total cost = {CalculateSalary(programmer.dateStart, programmer.dateEnd, teamProyect.salaryPercent, daysProgrammerMonth)}$).");
                    }
                }
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            
        }

        static decimal CalculateSalary(DateTime dateStart, DateTime dateEnd, decimal salaryPercent, int daysProgrammerMonth)
        {
            decimal salary = 0;
            salary = salaryPerDay * daysProgrammerMonth;

            if (salaryPercent != 100)
            {
                decimal discount = salary * (salaryPercent / 100);
                salary = salary - discount;
            }

            return salary;
        }
    }
}
