using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cars_K2_Example
{
    public interface IBetween<T>
    {
        /// <summary>
        /// Indicates whether the value of the certain property of the current instance is in
        /// [<paramref name="from"/>, <paramref name="to"/>] range including range marginal values.
        /// <paramref name="from"/> should always precede <paramref name="to"/> in default sort order.
        /// </summary>
        /// <param name="from">The starting value of the range</param>
        /// <param name="to">The ending value of the range</param>
        /// <returns>true if the value of the current object property is in range; otherwise,
        /// false.</returns>
        bool MutuallyInclusiveBetween(T from, T to);

        /// <summary>
        /// Indicates whether the value of the certain property of the current instance is in
        /// [<paramref name="from"/>, <paramref name="to"/>] range excluding range marginal values.
        /// <paramref name="from"/> should always precede <paramref name="to"/> in default sort order.
        /// </summary>
        /// <param name="from">The starting value of the range</param>
        /// <param name="to">The ending value of the range</param>
        /// <returns>true if the value of the current object property is in range; otherwise,
        /// false.</returns>
        bool MutuallyExclusiveBetween(T from, T to);
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            LinkList<Car> FirstList = InOut.ReadFromFile("Duomenys.txt");
            LinkList<Car> SecondList = new LinkList<Car>();
            LinkList<Car> ThirdList = new LinkList<Car>();
            LinkList<Car> FourthList = new LinkList<Car>();
            LinkList<Car> FifthList = new LinkList<Car>();

            if (File.Exists("Rezultatai.txt"))
                File.Delete("Rezultatai.txt");

            FirstList.Begin();
            if (FirstList.Exist())
            {
                double maxPrice = Task.MaxPrice(FirstList);

                Console.Write("Input first maker: "); 
                string FirstMaker = Console.ReadLine();
                Console.Write("Input second maker: ");
                string SecondMaker = Console.ReadLine();

                InOut.PrintToFile("Rezultatai.txt", "First list", FirstList);

                SecondList = Task.Filter<Car, string>(FirstList, FirstMaker, null);
                SecondList.Begin();
                if(SecondList.Exist())
                {
                    InOut.PrintToFile("Rezultatai.txt", "Second list with the first maker", SecondList);
                    FourthList = Task.Filter<Car, double>(SecondList, maxPrice - maxPrice / 100 * 25, maxPrice + maxPrice / 100 * 25);
                    FourthList.Begin();
                    if(FourthList.Exist())
                    {
                        FourthList.Sort();
                        InOut.PrintToFile("Rezultatai.txt", "Fourth list with 25% qualifier and for sorting", FourthList);
                    }
                    else
                    {
                        
                        File.AppendAllText("Rezultatai.txt", "Fourth list lacks data\n\r");
                    }
                }
                else
                {
                    File.AppendAllText("Rezultatai.txt", "Second list with the first maker lacks data\n\r");
                }

                ThirdList = Task.Filter<Car, string>(FirstList, null, SecondMaker);
                ThirdList.Begin();
                if (ThirdList.Exist())
                {
                    InOut.PrintToFile("Rezultatai.txt", "Third list with the second maker", ThirdList);
                    FifthList = Task.Filter<Car, double>(ThirdList, maxPrice - maxPrice / 100 * 25, maxPrice + maxPrice / 100 * 25);
                    FifthList.Begin();
                    if (FifthList.Exist())
                    {
                        FifthList.Sort();
                        InOut.PrintToFile("Rezultatai.txt", "Fifth list with 25% qualifier and for sorting", FifthList);
                    }
                    else
                    {
                        File.AppendAllText("Rezultatai.txt", "Fifth list lacks data\n\r");
                    }
                }
                else
                {
                    File.AppendAllText("Rezultatai.txt", "Third list with the second maker lacks data\n\r");
                }

                File.AppendAllText("Rezultatai.txt", $"Biggest price: {maxPrice}");
            }
            else
            {
                File.AppendAllText("Rezultatai.txt", "First list lacks data\n\r");
            }
        }
    }
    public class LinkList<T> where T : IComparable<T>
    {
        class Node
        {
            public T Data { get; set; }
            public Node Next { get; set; }
            public Node(T data, Node next)
            {
                Data = data;
                Next = next;
            }
        }

        /// <summary>
        /// All the time should point to the first element of the list.
        /// </summary>
        private Node begin;
        /// <summary>
        /// All the time should point to the last element of the list.
        /// </summary>
        private Node end;
        /// <summary>
        /// Shouldn't be used in any other methods except Begin(), Next(), Exist() and Get().
        /// </summary>
        private Node current;

        /// <summary>
        /// Initializes a new instance of the LinkList class with empty list.
        /// </summary>
        public LinkList()
        {
            begin = current = end = null;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to move internal pointer to the first element of the list.
        /// </summary>
        public void Begin()
        {
            current = begin;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to move internal pointer to the next element of the list.
        /// </summary>
        public void Next()
        {
            current = current.Next;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to check whether the internal pointer points to the element of the list.
        /// </summary>
        /// <returns>true, if the internal pointer points to some element of the list; otherwise,
        /// false.</returns>
        public bool Exist()
        {
            return current != null;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to get the value stored in the node pointed by the internal pointer.
        /// </summary>
        /// <returns>the value of the element that is pointed by the internal pointer.</returns>
        public T Get()
        {
            return current.Data;
        }

        /// <summary>
        /// Method appends new node to the end of the list and saves in it <paramref name="data"/>
        /// passed by the parameter.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="data">The data to be stored in the list.</param>
        public void Add(T data)
        {
            var gotten = new Node(data, null);
            if (begin != null)
            {
                end.Next = gotten;
                end = gotten;
            }
            else
            {
                begin = gotten;
                end = gotten;
            }
        }

        /// <summary>
        /// Method sorts data in the list. The data object class should implement IComparable
        /// interface though defining sort order.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        public void Sort()
        {
            for (Node d1 = begin; d1 != null; d1 = d1.Next)
            {
                Node max = d1;
                for (Node d2 = d1; d2 != null; d2 = d2.Next)
                {
                    if (d1.Data.CompareTo(d2.Data) > 0)
                    {
                        max = d2;
                        T temp = d1.Data;
                        d1.Data = max.Data;
                        max.Data = temp;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Provides properties and interface implementations for the storing and manipulating of cars data.
    /// THE STUDENT SHOULD DEFINE THE CLASS ACCORDING TO THE TASK.
    /// </summary>
    public class Car : IComparable<Car>, IBetween<string>, IBetween<double>
    {
        public string Maker { get; private set; }
        public string Model { get; private set; }
        public double Price { get; private set; }

        public Car(string maker, string model, double price)
        {
            Maker = maker;
            Model = model;
            Price = price;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer
        /// that indicates whether the current instance precedes, follows, or occurs in the same 
        /// position in the sort order as the other object.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The 
        /// return value has these meanings: -1 when this instance precedes other in the sort order;
        /// 0 when this instance occurs in the same position in the sort order as other;
        /// 1 when this instance follows other in the sort order.</returns>
        public int CompareTo(Car other)
        {
            if ((object)other == null)
                return 1;
            if (other.Price.CompareTo(this.Price) != 0)
            {
                return other.Price.CompareTo(this.Price);
            }
            else
            {
                return this.Model.CompareTo(other.Model);
            }
        }

        public bool MutuallyInclusiveBetween(string from, string to)
        {
            if(string.Compare(this.Maker,from)==0 || string.Compare(this.Maker,to)==0)
            {
                return true;
            }
            return false;
        }

        public bool MutuallyExclusiveBetween(string from, string to)
        {
            if (string.Compare(this.Maker, from) != 0 || string.Compare(this.Maker, to) != 0)
            {
                return true;
            }
            return false;
        }

        public bool MutuallyInclusiveBetween(double from, double to)
        {
            if(this.Price.CompareTo(from) > 0 && this.Price.CompareTo(to) < 0)
            {
                return true;
            }
            return false;
        }

        public bool MutuallyExclusiveBetween(double from, double to)
        {
            if (this.Price.CompareTo(from) < 0 || this.Price.CompareTo(to) > 0)
            {
                return true;
            }
            return false;
        }
    }


    public static class InOut
    {
        /// <summary>
        /// Creates the list containing data read from the text file.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="fileName">The name of the text file</param>
        /// <returns>List with data from file</returns>
        public static LinkList<Car> ReadFromFile(string fileName)
        {
            LinkList<Car> list = new LinkList<Car>();
            string line;
            using (var file = new StreamReader(fileName, Encoding.UTF8))
            {
                while ((line = file.ReadLine()) != null)
                {
                    var values = Regex.Split(line, "; ");
                    string maker = values[0];
                    string model = values[1];
                    double price = double.Parse(values[2]);

                    Car current = new Car(maker, model, price);
                    list.Add(current);
                }
            }
            return list;
        }

        /// <summary>
        /// Appends the table, built from data contained in the list and preceded by the header,
        /// to the end of the text file.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="fileName">The name of the text file</param>
        /// <param name="header">The header of the table</param>
        /// <param name="list">The list from which the table to be formed</param>
        public static void PrintToFile(string fileName, string header, LinkList<Car> list)
        {
            using (var file = new StreamWriter(fileName, true))
            {
                file.WriteLine(new string('-', 45));
                file.WriteLine(header);
                file.WriteLine(new string('-', 45));
                file.WriteLine($"{"Maker",-18} | {"Model",-10} | {"Price",-10} |");
                file.WriteLine(new string('-', 45));
                for (list.Begin(); list.Exist(); list.Next())
                {
                    Car current = list.Get();
                    file.WriteLine($"{current.Maker,-18} | {current.Model,-10} | {current.Price,10} |");
                }
                file.WriteLine(new string('-', 45));
                file.WriteLine();
            }
        }
    }

    public static class Task
    {
        /// <summary>
        /// The method finds the biggest price value in the given list.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="list">The data list to be searched.</param>
        /// <returns>The biggest price value.</returns>
        public static double MaxPrice(LinkList<Car> list)
        {
            double max = double.MinValue;
            for (list.Begin(); list.Exist(); list.Next())
            {
                Car current = list.Get();
                if (current.Price > max)
                {
                    max = current.Price;
                }
            }
            return max;
        }

        /// <summary>
        /// Filters data from the source list that meets filtering criteria and writes them
        /// into the new list.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// THE STUDENT SHOULDN'T CHANGE THE SIGNATURE OF THE METHOD!
        /// </summary>
        /// <typeparam name="TData">The type of the data objects stored in the list</typeparam>
        /// <typeparam name="TCriteria">The type of criteria</typeparam>
        /// <param name="source">The source list from which the result would be created</param>
        /// <param name="from">Lower bound of the interval</param>
        /// <param name="to">Upper bound of the interval</param>
        /// <returns>The list that contains filtered data</returns>
        public static LinkList<TData> Filter<TData, TCriteria>(LinkList<TData> source, TCriteria from, TCriteria to) where TData : IComparable<TData>, IBetween<TCriteria>
        {
            LinkList<TData> list = new LinkList<TData>();
            for (source.Begin(); source.Exist(); source.Next())
            {
                TData current = source.Get();
                if (current.MutuallyInclusiveBetween(from, to))
                {
                    list.Add(current);
                }

            }
            return list;
        }

    }
}
