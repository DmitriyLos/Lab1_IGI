using Lab1.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System;
using System.Linq;

namespace EFCore_LINQ.Models
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (STOContext db = new STOContext())
            {
                DbInitializer.Initialize(db);
                Console.WriteLine("====== Будет выполнена выборка данных (нажмите любую клавишу) ========");
                Console.ReadKey();
                SelectAllCars(db);
                Console.WriteLine("====== Будет выполнена выборка данных (нажмите любую клавишу) ========");
                Console.ReadKey();
                SelectAllOwners(db);
                Console.WriteLine("====== Будет выполнена выборка данных (нажмите любую клавишу) ========");
                Console.ReadKey();
                SelectCertaineWorker(db);
                Console.WriteLine("====== Будет выполнена выборка данных (нажмите любую клавишу) ========");
                Console.ReadKey();
                SelectBeforeCertainDate(db);
                Console.WriteLine("====== Будет выполнена выборка данных (нажмите любую клавишу) ========");
                Console.ReadKey();
                SelectCarsInThisYear(db);
                Console.WriteLine("====== Будет выполнена выборка данных (нажмите любую клавишу) ========");
                Console.ReadKey();
                SelectUncompletedCars(db);
                Console.WriteLine("====== Будет выполнено обновление данных (нажмите любую клавишу) ========");
                Console.ReadKey();
                Update(db);
                Console.WriteLine("====== Будет выполнено удаление данных (нажмите любую клавишу) ========");
                Console.ReadKey();
                Delete(db);
                Console.ReadKey();
            }
        }
                static void SelectAllWorkers(STOContext db)
                {
                    Console.WriteLine("1. Вывод списка работников\n");
                    var AllWorkers = db.Workers.ToList();
                    foreach (Worker worker in AllWorkers)
                        Console.WriteLine(value: $"ФИО: {worker.fioWorker} Дата приёма на работу: {worker.dateOfEmployment} Зарплата: {worker.salary}");
                    Console.ReadKey();
                }
                static void SelectAllCars(STOContext db)
                {
                    Console.WriteLine("1. Вывод полных сведений об автомобилях\n");
                    Console.WriteLine("Какой автомобиль: ");
                    var cars = db.Cars.ToList();
                    foreach (Car car in cars)
                        Console.WriteLine(value: $"Модель: {car.model} Мощность: {car.power} Цвет: {car.colour} Госномер: {car.stateNumber} Год выпуска: {car.yearOfIssue} Номер кузова: {car.bodyNumber} Номер двигателя: {car.engineNumber}");
                    Console.ReadKey();
                }

                static void SelectAllOwners(STOContext db)
                {
                    Console.WriteLine("2. Вывод полных сведений о владельцах\n");
                    Console.WriteLine("Владелец: ");
                    var owners = db.Owners.ToList();
                    foreach (Owner owner in owners)
                        Console.WriteLine(value: $"Водительское удостоверение: {owner.driverLicense} ФИО: {owner.fioOwner} Адрес: {owner.adress} Телефон: {owner.phone}");
                    Console.ReadKey();
                }

        static void SelectCertaineWorker(STOContext db)
        {
            DateTime startDate = new DateTime(2000, 01, 01, 0, 00, 00);
            DateTime endDate = new DateTime(2016, 01, 01, 0, 00, 00);
            string opredWork = "nJRMPVXokbNZaoR";


            var queryLINQ1 = from f in db.Cars
                             join t in db.Orders
                             on f.carID equals t.carID
                             join c in db.Workers
                             on t.workerID equals c.workerID
                             where (t.dateReceipt > startDate && t.dateCompletion < endDate && c.fioWorker == opredWork)
                             orderby f.carID descending
                             select new
                             {
                                 Модель = f.model,
                                 Мощность = f.power,
                                 Цвет = f.colour,
                                 Госномер = f.stateNumber
                             };
            string comment = "\n3. Выполненные заказы за заданный промежуток времени работником nJRMPVXokbNZaoR: \r\n";
            Print(comment, queryLINQ1.Take(5).ToList());
        }

        static void SelectBeforeCertainDate(STOContext db)
        {
            DateTime opredDate = new DateTime(2016, 01, 01, 0, 00, 00);
            var queryLINQ2 = from f in db.Cars
                             join t in db.Orders
                             on f.carID equals t.carID
                             where (t.dateReceipt < opredDate)
                             orderby f.carID descending
                             select new
                             {
                                 Модель = f.model,
                                 Мощность = f.power,
                                 Цвет = f.colour,
                                 Госномер = f.stateNumber
                             };
            string comment1 = "\n4. Сведения об автомобилях,поступивших в мастерскую до определенной даты: \r\n";
            Print(comment1, queryLINQ2.Take(5).ToList());
        }
        static void SelectCarsInThisYear(STOContext db)
        {
            var queryLINQ3 = from f in db.Cars
                             join t in db.Orders
                             on f.carID equals t.carID
                             where (t.dateReceipt.Year == DateTime.Now.Year)
                             orderby f.carID descending
                             select new
                             {
                                 Модель = f.model,
                                 Мощность = f.power,
                                 Цвет = f.colour,
                                 Госномер = f.stateNumber
                             };
            string comment2 = "\n5. Данные об автомобилях, поступивших в мастерскую в текущем году: \r\n";
            Print(comment2, queryLINQ3.Take(5).ToList());
        }
        static void SelectUncompletedCars(STOContext db)
        {
            var queryLINQ4 = from f in db.Cars
                                 join t in db.Orders
                                 on f.carID equals t.carID
                                 where (t.dateCompletion == null)
                                 orderby f.carID descending
                                 select new
                                 {
                                     Модель = f.model,
                                     Мощность = f.power,
                                     Цвет = f.colour,
                                     Госномер = f.stateNumber
                                 };
                string comment3 = "\n6. Cведения об автомобилях, работа над которыми еще не выполнена на текущую дату: \r\n";
                Print(comment3, queryLINQ4.Take(5).ToList());
        }

        static void Print(string sqltext, IEnumerable items)
        {
            Console.WriteLine(sqltext);
            Console.WriteLine("Записи: ");
            foreach(var item in items)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine();
            Console.ReadKey();
        }

        static void Insert(STOContext db)
        {
            Owner owner = new Owner
            {
                driverLicense = 228,
                fioOwner = "Adam",
                adress = "Cologne",
                phone = 1488
            };
            
            Car car = new Car
            {
                 model = "Peugeot",
                 power = 200,
                 colour = "Silver",
                 stateNumber = "9844EB3",
                 yearOfIssue = 2003,
                 bodyNumber = 2281488,
                 engineNumber = 1488228
            };
            

            Worker worker = new Worker
            {
                fioWorker = "Лось Д.И.",
                dateOfEmployment = DateTime.Now,
                salary = 293,
                postID = 7,
            };

            db.Workers.Add(worker);
            db.Owners.Add(owner);
            db.Cars.Add(car);
            db.SaveChanges();


        }
        static void Delete(STOContext db)
        {
            string nameWorker = "Лось Д.И.";
            var work = db.Workers.Where(c => c.fioWorker == nameWorker);

            string nameCar = "Peugeot";
            var car = db.Cars.Where(c => c.model == nameCar);

            db.Workers.RemoveRange(work);
            db.Cars.RemoveRange(car);
            Console.WriteLine("Удаление данных произведено успешно");
            db.SaveChanges();

        }
        static void Update(STOContext db)
        {
            decimal salary = 293;
            var sal = db.Workers.Where(c => c.salary == salary).FirstOrDefault();
            if (sal != null)
            {
                sal.salary = 350;
            };
            Console.WriteLine("Обновление данных произведено успешно");
            db.SaveChanges();

        }


    }
}