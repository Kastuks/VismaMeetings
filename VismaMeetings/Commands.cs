using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VismaMeetings
{
    internal class Commands
    {
        public static void CallCreate()
        {
            string name = "";
            List<string> names = new List<string>();
            Console.WriteLine("\t Write names for the meeting, press enter after every name. If done, write end");
            Console.Write("Your selection: ");
            while ((name = Console.ReadLine()) != "end")
            {
                Console.Write("Your selection: ");
                names.Add(name);
            }

            Console.WriteLine("\t Write Responsible persons name: ");
            Console.Write("Your selection: ");
            string responsiblePerson = Console.ReadLine();
            if (!names.Contains(responsiblePerson))
                names.Add(responsiblePerson);
            Console.WriteLine("\t Write a description for meeting: ");
            string description = Console.ReadLine();
            Console.WriteLine("\t Select a category: ");
            Console.WriteLine("\t 1 - CodeMonkey, 2 - Hub, 3 - Short, 4 - TeamBuilding");
            Console.Write("Your selection: ");

            string selection_str = Console.ReadLine();
            int selection = Convert.ToInt32(selection_str);

            Category cate = Category.Hub;
            if (selection == 1)
                cate = Category.CodeMonkey;
            else if (selection == 2)
                cate = Category.Hub;
            else if (selection == 3)
                cate = Category.Short;
            else if (selection == 4)
                cate = Category.TeamBuilding;

            Console.WriteLine("\t Select a type: ");
            Console.WriteLine("\t 1 - Live, 2 - InPerson");
            Console.Write("Your selection: ");

            string selection_str2 = Console.ReadLine();
            int selection2 = Convert.ToInt32(selection_str2);

            Type type = Type.Live;
            if (selection2 == 1)
                type = Type.Live;
            else if (selection == 2)
                type = Type.InPerson;

            Console.WriteLine("\t Enter the start date and then the end date in format \"dd-MM-yyyy\" ");
            Console.Write("Your selection: ");

            string format = "dd-MM-yyyy";
            string input1 = Console.ReadLine();

            Console.Write("Your selection: ");

            string input2 = Console.ReadLine();


            DateTime date1 = DateTime.ParseExact(input1, format, System.Globalization.CultureInfo.InvariantCulture);
            DateTime date2 = DateTime.ParseExact(input2, format, System.Globalization.CultureInfo.InvariantCulture);


            CreateMeeting(names, responsiblePerson, description, cate, type, date1, date2);
        }
        public static void CallDeleteMeeting()
        {
            Console.WriteLine("\t Meeting name: ");
            Console.Write("Your selection: ");

            string meet = Console.ReadLine();
            Console.WriteLine("\t Person responsible: ");
            Console.Write("Your selection: ");

            string respName = Console.ReadLine();
            DeleteMeeting(meet, respName);
        }

        public static void CallDeletePerson()
        {
            Console.WriteLine("\t Meeting name: ");
            Console.Write("Your selection: ");

            string meetName = Console.ReadLine();
            Console.WriteLine("\t Person to delete from meeting: ");
            Console.Write("Your selection: ");

            string personToDelete = Console.ReadLine();
            DeletePerson(meetName, personToDelete);


        }

        public static void CallAddPerson()
        {
            Console.WriteLine("\t Meeting name: ");
            Console.Write("Your selection: ");

            string meetName1 = Console.ReadLine();
            Console.WriteLine("\t Person to add to meeting: ");
            Console.Write("Your selection: ");

            string personToAdd = Console.ReadLine();
            AddPerson(meetName1, personToAdd);
        }

        public static void CallListMeetings()
        {
            Console.WriteLine("\t1 - Filter by description");
            Console.WriteLine("\t2 - Filter by responsible person");
            Console.WriteLine("\t3 - Filter by category");
            Console.WriteLine("\t4 - Filter by type");
            Console.WriteLine("\t5 - Filter by dates");
            Console.WriteLine("\t6 - Filter by the number of attendees");
            Console.WriteLine("\t7 - List all meetings");
            Console.Write("Your selection: ");
            int intSelection = Convert.ToInt32(Console.ReadLine());

            ListMeeting(intSelection);
        }

        static async void CreateMeeting(
            List<string> name,
            string responsiblePerson,
            string description,
            Category category,
            Type type,
            DateTime startDate,
            DateTime endDate)
        {
            Data _data = new Data()
            {
                Name = name,
                ResponsiblePerson = responsiblePerson,
                Description = description,
                Category = category,
                Type = type,
                StartDate = startDate,
                EndDate = endDate
            };

            string path = AppContext.BaseDirectory;
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\"));
            string pathString = System.IO.Path.Combine(newPath, "Meetings");

            if (!System.IO.File.Exists(pathString))
                System.IO.Directory.CreateDirectory(pathString);

            string pathToMeeting = System.IO.Path.Combine(pathString, description + ".json");

            using FileStream fileStream = File.Create(pathToMeeting);
            await JsonSerializer.SerializeAsync(fileStream, _data);


        }
        static void DeleteMeeting(string meeting, string responsiblePerson)
        {
            string path = AppContext.BaseDirectory;
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\"));
            string pathString = System.IO.Path.Combine(newPath, "Meetings");

            string pathToMeeting = System.IO.Path.Combine(pathString, meeting + ".json");

            if (System.IO.File.Exists(pathToMeeting))
            {

                string jsonString = File.ReadAllText(pathToMeeting);
                Console.WriteLine(jsonString);
                Data data = JsonSerializer.Deserialize<Data>(jsonString)!;

                Console.WriteLine(data);
                if (data.ResponsiblePerson == responsiblePerson)
                {
                    Console.WriteLine("ready to delete");
                    File.Delete(pathToMeeting);
                }
                else { Console.WriteLine("Wrong responsible person"); }
                //File.Delete()
            }
            else
                Console.WriteLine("Could not find meeting");

        }
        static async void AddPerson(string meeting, string personToAdd)
        {
            string path = AppContext.BaseDirectory;
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\"));
            string pathString = System.IO.Path.Combine(newPath, "Meetings");

            string pathToMeeting = System.IO.Path.Combine(pathString, meeting + ".json");

            DirectoryInfo d = new DirectoryInfo(pathString);

            FileInfo[] Files = d.GetFiles("*.json"); //Getting json files

            if (System.IO.File.Exists(pathToMeeting))
            {
                string jsonString = File.ReadAllText(pathToMeeting);
                //Console.WriteLine(jsonString);
                Data data = JsonSerializer.Deserialize<Data>(jsonString)!;
                bool checker = true;
                if (data.ResponsiblePerson == personToAdd)
                {
                    Console.WriteLine("Can't add a person who is already in the meeting");
                }
                else
                {
                    foreach (string person in data.Name.ToList())
                    {
                        if (personToAdd == person)
                        {
                            checker = false;
                            Console.WriteLine("Can't add a person who is already in the meeting");
                            //break;
                        }
                    }
                    if (checker)
                    {
                        data.Name.Add(personToAdd);
                        using FileStream fileStream = File.Create(pathToMeeting);
                        await JsonSerializer.SerializeAsync(fileStream, data);
                        Console.WriteLine("Successfully added person to the meeting at: " + DateTime.Now);
                        foreach (FileInfo file in Files)
                        {
                            string pathToMeeting2 = System.IO.Path.Combine(pathString, file.Name);

                            string jsonString2 = File.ReadAllText(pathToMeeting2);
                            //Console.WriteLine(jsonString);
                            Data data2 = JsonSerializer.Deserialize<Data>(jsonString2)!;



                            if (data2.Name.Contains(personToAdd) && ((data2.StartDate <= data.EndDate && data2.EndDate >= data.StartDate)))
                                Console.WriteLine("This person already has another scheduled meeting at this meetings time");

                        }
                    }
                }
            }
        }

        static async void DeletePerson(string meeting, string personToDelete)
        {
            string path = AppContext.BaseDirectory;
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\"));
            string pathString = System.IO.Path.Combine(newPath, "Meetings");

            string pathToMeeting = System.IO.Path.Combine(pathString, meeting + ".json");

            if (System.IO.File.Exists(pathToMeeting))
            {
                string jsonString = File.ReadAllText(pathToMeeting);
                //Console.WriteLine(jsonString);
                Data data = JsonSerializer.Deserialize<Data>(jsonString)!;

                if (data.ResponsiblePerson == personToDelete)
                {
                    Console.WriteLine("Can't delete responsible person");

                }
                else
                {
                    foreach (string person in data.Name.ToList())
                    {
                        //Console.WriteLine(person);
                        if (personToDelete == person)
                        {
                            try
                            {
                                data.Name.Remove(personToDelete);
                                Console.WriteLine("Success");
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine("Error " + e.Message);
                            }
                        }
                    }
                    using FileStream fileStream = File.Create(pathToMeeting);
                    await JsonSerializer.SerializeAsync(fileStream, data);
                }
            }
        }

        static void ListMeeting(int selection)
        {
            string path = AppContext.BaseDirectory;
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\"));
            string pathString = System.IO.Path.Combine(newPath, "Meetings");

            //string pathToMeeting = System.IO.Path.Combine(pathString, meeting + ".json");

            DirectoryInfo d = new DirectoryInfo(pathString); //Assuming Test is your Folder

            FileInfo[] Files = d.GetFiles("*.json"); //Getting Text files
            string str = "";
            if (selection == 1)
            {
                Console.WriteLine("Your description filter: ");
                string filter1 = Console.ReadLine();

                foreach (FileInfo file in Files)
                {
                    if (file.Name.Contains(filter1))
                    {
                        str = str + ", " + file.Name;
                    }
                }
            }
            else if (selection == 2)
            {
                Console.WriteLine("Your responsible person: ");
                string filter2 = Console.ReadLine();


                foreach (FileInfo file in Files)
                {
                    string pathToMeeting = System.IO.Path.Combine(pathString, file.Name);

                    string jsonString = File.ReadAllText(pathToMeeting);
                    //Console.WriteLine(jsonString);
                    Data data = JsonSerializer.Deserialize<Data>(jsonString)!;



                    if (data.ResponsiblePerson == filter2)
                    {
                        str = str + ", " + file.Name;
                    }
                }

            }
            else if (selection == 3)
            {
                Console.WriteLine("\t Categories - CodeMonkey / Hub / Short / TeamBuilding");
                Console.WriteLine("Your category: ");
                string filter3 = Console.ReadLine();


                foreach (FileInfo file in Files)
                {
                    string pathToMeeting = System.IO.Path.Combine(pathString, file.Name);

                    string jsonString = File.ReadAllText(pathToMeeting);
                    //Console.WriteLine(jsonString);
                    Data data = JsonSerializer.Deserialize<Data>(jsonString)!;



                    if (data.Category.ToString() == filter3)
                    {
                        str = str + ", " + file.Name;
                    }
                }


            }
            else if (selection == 4)
            {
                Console.WriteLine("\t Types - Live / InPerson");
                Console.WriteLine("Your type: ");
                string filter4 = Console.ReadLine();


                foreach (FileInfo file in Files)
                {
                    string pathToMeeting = System.IO.Path.Combine(pathString, file.Name);

                    string jsonString = File.ReadAllText(pathToMeeting);
                    //Console.WriteLine(jsonString);
                    Data data = JsonSerializer.Deserialize<Data>(jsonString)!;



                    if (data.Type.ToString() == filter4)
                    {
                        str = str + ", " + file.Name;
                    }
                }


            }
            else if (selection == 5)
            {
                DateTime date1 = DateTime.Now;
                DateTime date2 = DateTime.Now;
                Console.WriteLine("\t 1. Filter from date to now ");
                Console.WriteLine("\t 2. Filter two dates ");
                int filter5 = Convert.ToInt32(Console.ReadLine());


                if (filter5 == 1)
                {
                    Console.WriteLine("\t Enter the start date in format \"dd-MM-yyyy\" ");
                    string format = "dd-MM-yyyy";
                    string input1 = Console.ReadLine();

                    date1 = DateTime.ParseExact(input1, format, System.Globalization.CultureInfo.InvariantCulture);
                    date2 = DateTime.Now;

                }
                else if (filter5 == 2)
                {
                    Console.WriteLine("\t Enter the start date and then the end date in format \"dd-MM-yyyy\" ");
                    string format = "dd-MM-yyyy";
                    string input1 = Console.ReadLine();
                    string input2 = Console.ReadLine();

                    date1 = DateTime.ParseExact(input1, format, System.Globalization.CultureInfo.InvariantCulture);
                    date2 = DateTime.ParseExact(input2, format, System.Globalization.CultureInfo.InvariantCulture);

                }


                foreach (FileInfo file in Files)
                {
                    string pathToMeeting = System.IO.Path.Combine(pathString, file.Name);

                    string jsonString = File.ReadAllText(pathToMeeting);
                    //Console.WriteLine(jsonString);
                    Data data = JsonSerializer.Deserialize<Data>(jsonString)!;



                    if (data.StartDate >= date1 && data.EndDate <= date2)
                    {
                        str = str + ", " + file.Name;
                    }
                }


            }

            else if (selection == 6)
            {
                Console.WriteLine("Minimum amount of atendees to filter: ");
                int filter4 = Convert.ToInt32(Console.ReadLine());


                foreach (FileInfo file in Files)
                {
                    string pathToMeeting = System.IO.Path.Combine(pathString, file.Name);

                    string jsonString = File.ReadAllText(pathToMeeting);
                    //Console.WriteLine(jsonString);
                    Data data = JsonSerializer.Deserialize<Data>(jsonString)!;



                    if (data.Name.Count() >= filter4)
                    {
                        str = str + ", " + file.Name;
                    }
                }


            }
            else if (selection == 7)
            {
                foreach (FileInfo file in Files)
                {
                    string pathToMeeting = System.IO.Path.Combine(pathString, file.Name);

                    string jsonString = File.ReadAllText(pathToMeeting);
                    //Console.WriteLine(jsonString);
                    Data data = JsonSerializer.Deserialize<Data>(jsonString)!;

                    str = str + ", " + file.Name;

                }

            }
            Console.WriteLine("Meetings found with your filter: \t" + str);

        }




    }
}
