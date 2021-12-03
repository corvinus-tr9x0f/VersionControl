using MicroSimWeek10.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSimWeek10
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();

        Random rng = new Random(1234);

        List<Person> Men = new List<Person>();
        List<Person> Women = new List<Person>();

        public Form1()
        {
            InitializeComponent();

            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");
        }

        private void Simulation(int endYear, string csvPath)
        {
            txtMain.Clear();
            Men.Clear();
            Women.Clear();

            Population = GetPopulation(csvPath);

            for (int year = 2005; year <= endYear; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);

                    if (Population[i].Gender == Gender.Male && Population[i].IsAlive == true)
                    {
                        Men.Add(new Person()
                        {
                            BirthYear = Population[i].BirthYear,
                            Gender = Population[i].Gender,
                            NbrOfChildren = Population[i].NbrOfChildren,
                            IsAlive = Population[i].IsAlive
                        });
                    }
                    else if (Population[i].Gender == Gender.Female && Population[i].IsAlive == true)
                    {
                        Women.Add(new Person()
                        {
                            BirthYear = Population[i].BirthYear,
                            Gender = Population[i].Gender,
                            NbrOfChildren = Population[i].NbrOfChildren,
                            IsAlive = Population[i].IsAlive
                        });
                    }

                    int nbrOfMales = (from x in Population
                                      where x.Gender == Gender.Male && x.IsAlive
                                      select x).Count();
                    int nbrOfFemales = (from x in Population
                                        where x.Gender == Gender.Female && x.IsAlive
                                        select x).Count();

                    txtMain.Text += string.Format(
                        string.Format("Szimulációs év:{0}\n\t Fiúk:{1} \n\tLányok:{2}\n\n",
                        year,
                        nbrOfMales,
                        nbrOfFemales));
                }
            }
        }

        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var sor = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(sor[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), sor[1]),
                        NbrOfChildren = int.Parse(sor[2])
                    });
                }
            }
            return population;
        }

        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> birthprobabilities = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var sor = sr.ReadLine().Split(';');
                    birthprobabilities.Add(new BirthProbability()
                    {
                        Age = int.Parse(sor[0]),
                        NbrOfChildren = int.Parse(sor[1]),
                        P = double.Parse(sor[2])
                    });
                }
            }
            return birthprobabilities;
        }

        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> deathprobabilities = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var sor = sr.ReadLine().Split(';');
                    deathprobabilities.Add(new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), sor[0]),
                        Age = int.Parse(sor[1]),
                        P = double.Parse(sor[2])
                    });
                }
            }
            return deathprobabilities;
        }

        private void SimStep(int year, Person person)
        {
            //Ha halott akkor kihagyjuk, ugrunk a ciklus következő lépésére
            if (!person.IsAlive) return;

            // Letároljuk az életkort, hogy ne kelljen mindenhol újraszámolni
            byte age = (byte)(year - person.BirthYear);

            // Halál kezelése
            // Halálozási valószínűség kikeresése
            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Age == age
                             select x.P).FirstOrDefault();
            // Meghal a személy?
            if (rng.NextDouble() <= pDeath)
                person.IsAlive = false;

            //Születés kezelése - csak az élő nők szülnek
            if (person.IsAlive && person.Gender == Gender.Female)
            {
                //Szülési valószínűség kikeresése
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.P).FirstOrDefault();
                //Születik gyermek?
                if (rng.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Simulation((int)numYear.Value, txtPath.Text);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.FileName = txtPath.Text;

            if (ofd.ShowDialog() != DialogResult.OK) return;
            txtPath.Text = ofd.FileName;
        }
    }
}
