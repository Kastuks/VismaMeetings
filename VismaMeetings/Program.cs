// See https://aka.ms/new-console-template for more information
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace VismaMeetings
{

    class Program
    {
        static void Main(string[] args)
        {
            bool endApp = false;

            while (!endApp)
            {
                Console.WriteLine("Choose an operator from the following list:");
                Console.WriteLine("\tc - Create Meeting");
                Console.WriteLine("\td - Delete Meeting");
                Console.WriteLine("\tu - Delete User");
                Console.WriteLine("\ta - Add User");
                Console.WriteLine("\tl - List Meetings");
                Console.WriteLine("\tq - Quit Program");
                Console.Write("Your selection: ");
                string op = Console.ReadLine();

                switch(op)
                {
                    case "c":
                        Commands.CallCreate();                       
                        break;
                    case "d":
                        Commands.CallDeleteMeeting();
                        break;
                    case "u":
                        Commands.CallDeletePerson();
                        break;
                    case "a":
                        Commands.CallAddPerson();
                        break;

                    case "l":
                        Commands.CallListMeetings();
                        break;

                    case "q":
                        endApp = true;
                        break;

                    default:
                        break;

                }
            
            
            }
        }


    }

}