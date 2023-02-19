using System;
using System.Collections.Generic;
using System.Linq;
using PhoneApp.Domain.Interfaces;
using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using Newtonsoft.Json;
using System.IO;

namespace EmployeesMoreLoader.Plugin
{
    [Author(Name = "Danila Vechersky")]
    public class Plugin : IPluggable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> args)
        {
            var employeesList = args.Cast<EmployeesDTO>().ToList();
            logger.Info("Start parse employeers");
            string textJson = File.ReadAllText(@"..\..\Json users\users.json");
            var person = JsonConvert.DeserializeObject<Person>(textJson);
            for (int i = 0; i < person.users.Length; i++)
            {
                string name = person.users[i].firstName;
                string phone = person.users[i].phone;
                employeesList.Add(new EmployeesDTO { Name = name });
                if (!String.IsNullOrEmpty(phone))
                    employeesList.Last().AddPhone(phone);
            }


            if(employeesList.Count < person.users.Length)
                logger.Warn("Not all users are uploaded");
            else 
                logger.Info($"Uploaded {person.users.Length} users");

            return employeesList.Cast<DataTransferObject>();
        }
    }


}