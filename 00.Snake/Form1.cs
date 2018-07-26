using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Configuration;
using System.Xml;
using System.Reflection;


namespace _00.Snake
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
        }


        private static string getConfigFilePath()//Шукаємо файл конфігурації
        {
            return Assembly.GetExecutingAssembly().Location + ".config";
        }

        private static XmlDocument loadConfigDocument()//Завантажуємо файл конфігурації
        {
            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument();
                doc.Load(getConfigFilePath());
                return doc;
            }
            catch (System.IO.FileNotFoundException e)
            {
                throw new Exception("No configuration file found.", e);
            }
        }

        public static void WriteSetting(string key, string value)//Функція для запису до конфігураційного файлу
        {
            // Завантажуємо файл конфігурації
            XmlDocument doc = loadConfigDocument();

            // шукаємо блок appSettings
            XmlNode node = doc.SelectSingleNode("//appSettings");

            if (node == null)
                throw new InvalidOperationException("appSettings section not found in config file.");

            try
            {
                // шукаємо елемент за вказаним ключем
                XmlElement elem = (XmlElement)node.SelectSingleNode(string.Format("//add[@key='{0}']", key));

                if (elem != null)
                {
                    // додаємо значення до знайденого ключа
                    elem.SetAttribute("value", value);
                }
                else
                {
                    // якщо такий ключ не знайдено то створюємо його
                    // і встановлюємо значення
                    elem = doc.CreateElement("add");
                    elem.SetAttribute("key", key);
                    elem.SetAttribute("value", value);
                    node.AppendChild(elem);
                }
                doc.Save(getConfigFilePath());
            }
            catch
            {
                throw;
            }
        }


        Bitmap wall = _00.Snake.Properties.Resources.wall;                 //Завантаження зображень із ресурсів
        Bitmap food = _00.Snake.Properties.Resources.food;                 //
        Bitmap free = _00.Snake.Properties.Resources.free;                 //
        Bitmap injury = _00.Snake.Properties.Resources.injury;             //
        Bitmap node = _00.Snake.Properties.Resources.node;                 //
        Bitmap button1pick = _00.Snake.Properties.Resources.button1pick;   //
        Bitmap button1MD = _00.Snake.Properties.Resources.button1MD;       //

        public void Out(map field)//Функція для виводу ігрового поля на єкран
        {
            Bitmap img = new Bitmap(400, 400);
            Graphics g = Graphics.FromImage(img);
            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 20; j++)
                {
                    if (field.step[i, j].value == -1) { g.DrawImage(wall, new Point(20 * i, 20 * j)); }
                    if (field.step[i, j].value == 0) { g.DrawImage(free, new Point(20 * i, 20 * j)); }
                    if (field.step[i, j].value == 10) { g.DrawImage(food, new Point(20 * i, 20 * j)); }
                    if (field.step[i, j].value == -10) { g.DrawImage(injury, new Point(20 * i, 20 * j)); }
                    if (field.step[i, j].value == 1) { g.DrawImage(node, new Point(20 * i, 20 * j)); }
                }
            pictureBox1.Image = img;
        }

        map mainM = new map();
        Snake snake = new Snake();
        string bestT = ConfigurationSettings.AppSettings["Best"];
        int best = 0;

        private void Form1_Load(object sender, EventArgs e)//Програш звуку та завантаження рекорду
        {
            System.Media.SoundPlayer Audio;
            Audio = new System.Media.SoundPlayer(Properties.Resources.main);
            Audio.Load();
            Audio.PlayLooping();
            Int32.TryParse(bestT, out best);
            label2.Text = best.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e) //Один крок змійки
        {
            if (snake.alive)  //Якщо змійка жива
            {
                snake.Move(mainM);//то робимо крок
                Out(mainM);//і виводимо результат на екран
            }
            label3.Text = (snake.length).ToString();//Виводимо поточну довжину змійки
            if (snake.length > best)//Змінюємо колір, якщо встановлено новий рекорд
            {
                label3.ForeColor = Color.LawnGreen;
                label4.ForeColor = Color.LawnGreen;
            }
            if (snake.alive == false && snake.length > best)//Запам'ятовуємо новий реорд після смерті змійки
            {
                best = snake.length;
                WriteSetting("Best", best.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)//Починаємо нову гру
        {
            snake.SnakeReincarnation();//Змійка перероджується
            snake.length = 5;
            mainM.GenMap();
            label3.ForeColor = Color.White;
            label4.ForeColor = Color.White;
            label2.Text = best.ToString();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)//Змійка змінює напрям руху
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up) { mainM.way = "U"; }
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left) { mainM.way = "L"; }
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down) { mainM.way = "D"; }
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right) { mainM.way = "R"; }
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)        //Зміна зображення кнопки при натисканні
        {                                                                      //
            button1.Image = button1MD;                                         //
        }                                                                      //
                                                                               //
        private void button1_MouseUp(object sender, MouseEventArgs e)          //
        {                                                                      //
            button1.Image = button1pick;                                       //
        }                                                                      //




    }
}
