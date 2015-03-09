using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Media;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Snake
{
    public class Game
    {
        public Game(int height, int width)
        {

            // varijable i imena funkcija sa indeksom _2 označavaju da se radi o svostvima i funkcijama za AI zmiju

            game_width = width;
            game_height = height;

            snake_area_x = new Linkedlist();
            snake_area_y = new Linkedlist();

            snake_area_x_2 = new Linkedlist();
            snake_area_y_2 = new Linkedlist();

            wall_area_x = new Linkedlist();
            wall_area_y = new Linkedlist();

            snake_skin_color = Brushes.Green;
            snake_body_color = Brushes.Crimson;

            snake_skin_color_2 = Brushes.Orange;
            snake_body_color_2 = Brushes.Olive;

            back_color = Brushes.Black;

            wall_skin_color = Brushes.DarkGoldenrod;
            wall_body_color = Brushes.Chocolate;

            bait_bool = false;
            bait_bool_inc = false;
            bait_bool_dec = false;
            bait_bool_lv = false;
            bait_bool_smjer = false;
            bait_bool_dulj = false;
            bait_bool_rec = false;

            bait_skin_color = Brushes.DeepSkyBlue;
            bait_body_color = Brushes.Blue;

            bait_skin_color_inc = Brushes.Purple;
            bait_body_color_inc = Brushes.White;

            bait_skin_color_smjer = Brushes.Aqua;
            bait_body_color_smjer = Brushes.White;

            bait_skin_color_dulj = Brushes.Brown;
            bait_body_color_dulj = Brushes.Yellow;

            bait_skin_color_dec = Brushes.Yellow;
            bait_body_color_dec = Brushes.Purple;

            bait_body_color_lv = Brushes.Green;
            bait_skin_color_lv = Brushes.Purple;

            rectangle_color = Brushes.Green;

            node_length = 20;
            node_border_length = 2;

            bait.X = -1;
            bait.Y = -1;

            bait_inc.X = -1;
            bait_inc.Y = -1;

            bait_dec.X = -1;
            bait_dec.Y = -1;

            bait_lv.X = -1;
            bait_lv.Y = -1;

            bait_smjer.X = -1;
            bait_smjer.Y = -1;

            bait_dulj.X = -1;
            bait_dulj.Y = -1;

            bait_rec.X = -1;
            bait_rec.Y = -1;
            
            snake_route = 2;
            snake_route_2 = 2;
            snake_route_temp = 2;
            snake_route_temp_2 = 2;
            point = 0;
            level_point = 100;

            sound_eated = new SoundPlayer(Application.StartupPath + "\\sounds\\eated.wav");
            sound_menu = new SoundPlayer(Application.StartupPath + "\\sounds\\menu.wav");
            sound_gameover = new SoundPlayer(Application.StartupPath + "\\sounds\\gameover.wav");


            myfont = new PrivateFontCollection();
            myfont.AddFontFile(Application.StartupPath + "\\fonts\\Commodore Angled v1.2.ttf");


            fontfamily = myfont.Families[0];

            game_status = "menu_1";
            game_level = 1;
            max_level = 6;
            sound_control = "on";
            speed_control = "easy";
            snake_speed = new int[3] { 150, 100, 75 };
            


            // tipke za pomake da se ne sudari
            tipkaZaGore = "W";
            tipkaZaDolje = "S";
            tipkaZaLijevo = "A";
            tipkaZaDesno = "D";

            // broj dozvoljenih pauza
            brojDozPauza = 5;
            //broj iskoristenih pauza
            brojIskorPauza = 1;

            // broj dozvoljenih zivota
            brojDozZivota = 3;
            //broj iskoristenih zivota
            brojIskorZivota = 1;

        }

        public PrivateFontCollection myfont;
        public FontFamily fontfamily;

        public SoundPlayer sound_eated;
        public SoundPlayer sound_menu;
        public SoundPlayer sound_gameover;

        public int game_width;
        public int game_height;

        public Linkedlist snake_area_x;
        public Linkedlist snake_area_y;

        public Linkedlist snake_area_x_2;
        public Linkedlist snake_area_y_2;

        public Linkedlist wall_area_x;
        public Linkedlist wall_area_y;

        public Brush snake_skin_color;
        public Brush snake_body_color;

        public Brush snake_skin_color_2;
        public Brush snake_body_color_2;

        public Brush back_color;

        public Brush wall_skin_color;
        public Brush wall_body_color;

        public Point bait = new Point();
        public Point bait_inc = new Point();
        public Point bait_dec = new Point();
        public Point bait_lv = new Point();
        public Point bait_smjer = new Point();
        public Point bait_dulj = new Point();
        public Point bait_rec = new Point();

        public bool bait_bool;
        public bool eated;
        public bool eated2;
        public Brush bait_skin_color;
        public Brush bait_body_color;

        public bool bait_bool_inc;
        public Brush bait_skin_color_inc;
        public Brush bait_body_color_inc;

        public bool bait_bool_dec;
        public bool bait_bool_lv;
        public bool bait_bool_smjer;
        public bool bait_bool_dulj;
        public bool bait_bool_rec;

        public Brush bait_skin_color_dec;
        public Brush bait_body_color_dec;
        public Brush bait_body_color_lv;
        public Brush bait_skin_color_lv;
        public Brush bait_skin_color_smjer;
        public Brush bait_body_color_smjer;
        public Brush bait_skin_color_dulj;
        public Brush bait_body_color_dulj;

        public Brush rectangle_color;

        public int node_x;
        public int node_y;

        public int node_x_2;
        public int node_y_2;

        public int node_length;
        public int node_border_length;


        public int snake_length;
        public int snake_length_2;

        public int snake_route;
        public int snake_route_2;

        public int snake_route_temp;
        public int snake_route_temp_2;

        public int point;
        public int level_point;
        public int wait_time;

        public Bitmap bmp;
        public Graphics graph;
        public Bitmap menu_bmp;
        public Graphics menu_graph;

        public int key_control;
        public string game_status;
        public int game_level;
        public int max_level;
        public string sound_control;
        public string speed_control;
        public int[] snake_speed;
        public int[] snake_speed_2;

        //tipke za pomake da se ne sudari
        public string tipkaZaGore;
        public string tipkaZaDolje;
        public string tipkaZaLijevo;
        public string tipkaZaDesno;

        // broj dozvoljenih pauza
        public int brojDozPauza;
        // broj iskoristenih pauza
        public int brojIskorPauza;

        // broj dozvoljenih zivota
        public int brojDozZivota;
        // broj iskoristenih zivota
        public int brojIskorZivota;



        // zastavica koja sluzi da SHIFT + strijelia ne ude u beskonacnu petlju
        // ako zmija pojede specijalnu hranu za promijenu smijera
        public int smjerZastava = 0;



        // zastavice za oznacavanje jeli neka hrana stvorena i postavljena na igracu plocu
        // zastavica koja oznacava stavranje specijalne hrane za inc
        public int incPostavljen = 0;

        // zastavica koja oznacava stavranje specijalne hrane za dec
        public int decPostavljen = 0;

        // zastavica koja oznacava stavranje specijalne hrane za lv
        public int lvPostavljen = 0;

        // zastavica koja oznacava stavranje specijalne hrane za smjer
        public int smjerPostavljen = 0;

        // zastavica koja oznacava stavranje specijalne hrane za dulj
        public int duljPostavljen = 0;


        // booleani za provjeru jeli pojedena specijalna hrana prije gasenja specijalne hrane u odredenom trenutku
        // inace ako bi pojeli hranu i zmija prelazila preko tog podrucja a dosla sekunda u kojem se ta hrana gasi 
        // na zmiji bi ostao crni cvor
        public bool incEated = false;
        public bool decEated = false;
        public bool lvEated = false;
        public bool smjerEated = false;
        public bool duljEated = false;

        // za naizmjenicno kretanje dvije zmije na igracoj ploci
        public int br = 0;


        // brojac za kretanje kroz polja koordinata koje predaje AStar
        public int brojacKoord = 1;

        public AStarExample ase = new AStarExample();
        public AStarPathfinder pathfinder = new AStarPathfinder();


        // stvaranje bmp sa crnom pozadinom
        public void Load_Graphic()
        {
            bmp = new Bitmap(game_width, game_height);
            graph = Graphics.FromImage(bmp);
            graph.FillRectangle(back_color, 0, 0, game_width, game_height);
        }

        //dodavanje cvora zmiji kada pojede hranu
        public void Add_Snake_Node(int x, int y)
        {
            //(BOJA, X KOORD, Y KOORD, SIRINA, VISINA)
            graph.FillRectangle(snake_skin_color, x * node_length, y * node_length, node_length, node_length);
            graph.FillRectangle(snake_body_color, x * node_length, y * node_length, node_length - node_border_length, node_length - node_border_length);

        }


        public void Add_Snake_Node_2(int x, int y)
        {
            //(BOJA, X KOORD, Y KOORD, SIRINA, VISINA)
            graph.FillRectangle(snake_skin_color_2, x * node_length, y * node_length, node_length, node_length);
            graph.FillRectangle(snake_body_color_2, x * node_length, y * node_length, node_length - node_border_length, node_length - node_border_length);

        }


        // brisanje cvora zmije na odredenoj koordinati
        public void Delete_Snake_Node(int x, int y, int z)
        {
            if (z == 0)
            {
                graph.FillRectangle(back_color, x * node_length, y * node_length, node_length, node_length);
            }
            // ako su cvorovi u podrucju u kojem je dozvoljeno da zmija prelazi preko sebe
            else
            {
                graph.FillRectangle(rectangle_color, x * node_length, y * node_length, node_length, node_length);
            }
        }

        public void Delete_Snake_Node_2(int x, int y, int z)
        {
            if (z == 0)
            {
                graph.FillRectangle(back_color, x * node_length, y * node_length, node_length, node_length);
            }
            // ako su cvorovi u podrucju u kojem je dozvoljeno da zmija prelazi preko sebe
            else
            {
                graph.FillRectangle(rectangle_color, x * node_length, y * node_length, node_length, node_length);
            }
        }

        // smijer zmije
        public void Move_Snake_Left()
        {
            node_x--;
        }

        public void Move_Snake_Right()
        {
            node_x++;
        }

        public void Move_Snake_Up()
        {
            node_y--;
        }

        public void Move_Snake_Down()
        {
            node_y++;
        }

        //za drugu zmiju
        public void Move_Snake_Left_2()
        {
            node_x_2--;
        }

        public void Move_Snake_Right_2()
        {
            node_x_2++;
        }

        public void Move_Snake_Up_2()
        {
            node_y_2--;
        }

        public void Move_Snake_Down_2()
        {
            node_y_2++;
        }

        // kontroliranje obicne hrane ako je na istoj koordinati kao i prepreke ili tijelo zmije
        public bool Control_Bait_Coordinate()
        {
            Node temp_x = snake_area_x.first;
            Node temp_y = snake_area_y.first;

            Node temp_x_2 = snake_area_x_2.first;
            Node temp_y_2 = snake_area_y_2.first;

            for (int i = snake_area_x.length; i > 0; i--)
            {
                if ((bait.X == temp_x.value) && (bait.Y == temp_y.value))
                    return true;

                else
                {
                    temp_x = temp_x.next;
                    temp_y = temp_y.next;

                }
            }

            for (int i = snake_area_x_2.length; i > 0; i--)
            {
                if ((bait.X == temp_x_2.value) && (bait.Y == temp_y_2.value))
                    return true;

                else
                {
                    temp_x_2 = temp_x_2.next;
                    temp_y_2 = temp_y_2.next;

                }
            }

            temp_x = wall_area_x.first;
            temp_y = wall_area_y.first;

            for (int j = wall_area_x.length; j > 0; j--)
            {
                if ((bait.X == temp_x.value) && (bait.Y == temp_y.value))
                    return true;
                else
                {
                    temp_x = temp_x.next;
                    temp_y = temp_y.next;
                }
            }

            //ako je unutar specijalnog podrucja za prelazak zmije preko sebe
            if ((bait_rec.X <= bait.X) && (bait_rec.X + 6 >= bait.X)
                    && (bait_rec.Y <= bait.Y) && (bait_rec.Y + 6 >= bait.Y))
                return true;

            return false;
        }

        // kontroliranje hrane koja povecava bodove ako je na istoj koordinati kao i prepreke ili tijelo zmije
        public bool Control_Bait_Coordinate_inc()
        {
            Node temp_x = snake_area_x.first;
            Node temp_y = snake_area_y.first;

            for (int i = snake_area_x.length; i > 0; i--)
            {
                if ((bait_inc.X == temp_x.value) && (bait_inc.Y == temp_y.value))

                    return true;

                else
                {
                    temp_x = temp_x.next;
                    temp_y = temp_y.next;

                }
            }

            temp_x = wall_area_x.first;
            temp_y = wall_area_y.first;

            for (int j = wall_area_x.length; j > 0; j--)
            {
                if ((bait_inc.X == temp_x.value) && (bait_inc.Y == temp_y.value))
                    return true;
                else
                {
                    temp_x = temp_x.next;
                    temp_y = temp_y.next;
                }
            }


            Node temp_x_2 = snake_area_x_2.first;
            Node temp_y_2 = snake_area_y_2.first;

            for (int i = snake_area_x_2.length; i > 0; i--)
            {
                if ((bait_inc.X == temp_x_2.value) && (bait_inc.Y == temp_y_2.value))
                    return true;

                else
                {
                    temp_x_2 = temp_x_2.next;
                    temp_y_2 = temp_y_2.next;

                }
            }

            //ako je na istoj koordinati kao obicna hrana
            if ((bait_inc.X == bait.X) && (bait_inc.Y == bait.Y))
                return true;

            //ako je unutar specijalnog podrucja za prelazak zmije preko sebe
            if ((bait_rec.X <= bait_inc.X) && (bait_rec.X + 6 >= bait_inc.X)
                    && (bait_rec.Y <= bait_inc.Y) && (bait_rec.Y + 6 >= bait_inc.Y))
                return true;

            return false;
        }


        // kontroliranje hrane koja smanjuje bodove ako je na istoj koordinati kao i prepreke ili tijelo zmije
        public bool Control_Bait_Coordinate_dec()
        {
            Node temp_x = snake_area_x.first;
            Node temp_y = snake_area_y.first;

            for (int i = snake_area_x.length; i > 0; i--)
            {
                if ((bait_dec.X == temp_x.value) && (bait_dec.Y == temp_y.value))
                    return true;

                else
                {
                    temp_x = temp_x.next;
                    temp_y = temp_y.next;

                }
            }

            temp_x = wall_area_x.first;
            temp_y = wall_area_y.first;

            for (int j = wall_area_x.length; j > 0; j--)
            {
                if ((bait_dec.X == temp_x.value) && (bait_dec.Y == temp_y.value))
                    return true;
                else
                {
                    temp_x = temp_x.next;
                    temp_y = temp_y.next;
                }
            }

            Node temp_x_2 = snake_area_x_2.first;
            Node temp_y_2 = snake_area_y_2.first;

            for (int i = snake_area_x_2.length; i > 0; i--)
            {
                if ((bait_dec.X == temp_x_2.value) && (bait_dec.Y == temp_y_2.value))
                    return true;

                else
                {
                    temp_x_2 = temp_x_2.next;
                    temp_y_2 = temp_y_2.next;

                }
            }

            //ako je na istoj koordinati kao obicna hrana
            if ((bait_dec.X == bait.X) && (bait_dec.Y == bait.Y))
                return true;

            //ako je unutar specijalnog podrucja za prelazak zmije preko sebe
            if ((bait_rec.X <= bait_dec.X) && (bait_rec.X + 6 >= bait_dec.X)
                    && (bait_rec.Y <= bait_dec.Y) && (bait_rec.Y + 6 >= bait_dec.Y))
                return true;

            return false;
        }

        // kontroliranje hrane koja zavrzava level ako je na istoj koordinati kao i prepreke ili tijelo zmije
        public bool Control_Bait_Coordinate_lv()
        {
            Node temp_x = snake_area_x.first;
            Node temp_y = snake_area_y.first;

            for (int i = snake_area_x.length; i > 0; i--)
            {
                if ((bait_lv.X == temp_x.value) && (bait_lv.Y == temp_y.value))

                    return true;

                else
                {
                    temp_x = temp_x.next;
                    temp_y = temp_y.next;

                }
            }

            temp_x = wall_area_x.first;
            temp_y = wall_area_y.first;

            for (int j = wall_area_x.length; j > 0; j--)
            {
                if ((bait_lv.X == temp_x.value) && (bait_lv.Y == temp_y.value))
                    return true;
                else
                {
                    temp_x = temp_x.next;
                    temp_y = temp_y.next;
                }
            }

            Node temp_x_2 = snake_area_x_2.first;
            Node temp_y_2 = snake_area_y_2.first;

            for (int i = snake_area_x_2.length; i > 0; i--)
            {
                if ((bait_lv.X == temp_x_2.value) && (bait_lv.Y == temp_y_2.value))
                    return true;

                else
                {
                    temp_x_2 = temp_x_2.next;
                    temp_y_2 = temp_y_2.next;

                }
            }

            //ako je na istoj koordinati kao obicna hrana
            if ((bait_lv.X == bait.X) && (bait_lv.Y == bait.Y))
                return true;

            //ako je unutar specijalnog podrucja za prelazak zmije preko sebe
            if ((bait_rec.X <= bait_lv.X) && (bait_rec.X + 6 >= bait_lv.X)
                    && (bait_rec.Y <= bait_lv.Y) && (bait_rec.Y + 6 >= bait_lv.Y))
                return true;

            return false;
        }

        // kontroliranje hrane koja mijenja smijer ako je na istoj koordinati kao i prepreke ili tijelo zmije
        public bool Control_Bait_Coordinate_smjer()
        {
            Node temp_x = snake_area_x.first;
            Node temp_y = snake_area_y.first;

            for (int i = snake_area_x.length; i > 0; i--)
            {
                if ((bait_smjer.X == temp_x.value) && (bait_smjer.Y == temp_y.value))

                    return true;

                else
                {
                    temp_x = temp_x.next;
                    temp_y = temp_y.next;

                }
            }

            temp_x = wall_area_x.first;
            temp_y = wall_area_y.first;

            for (int j = wall_area_x.length; j > 0; j--)
            {
                if ((bait_smjer.X == temp_x.value) && (bait_smjer.Y == temp_y.value))
                    return true;
                else
                {
                    temp_x = temp_x.next;
                    temp_y = temp_y.next;
                }
            }


            Node temp_x_2 = snake_area_x_2.first;
            Node temp_y_2 = snake_area_y_2.first;

            for (int i = snake_area_x_2.length; i > 0; i--)
            {
                if ((bait_smjer.X == temp_x_2.value) && (bait_smjer.Y == temp_y_2.value))
                    return true;

                else
                {
                    temp_x_2 = temp_x_2.next;
                    temp_y_2 = temp_y_2.next;

                }
            }

            //ako je na istoj koordinati kao obicna hrana
            if ((bait_smjer.X == bait.X) && (bait_smjer.Y == bait.Y))
                return true;

            //ako je unutar specijalnog podrucja za prelazak zmije preko sebe
            if ((bait_rec.X <= bait_smjer.X) && (bait_rec.X + 6 >= bait_smjer.X)
                    && (bait_rec.Y <= bait_smjer.Y) && (bait_rec.Y + 6 >= bait_smjer.Y))
                return true;

            return false;
        }

        // kontroliranje hrane za duljinu ako je na istoj koordinati kao i prepreke ili tijelo zmije
        public bool Control_Bait_Coordinate_dulj()
        {
            Node temp_x = snake_area_x.first;
            Node temp_y = snake_area_y.first;

            for (int i = snake_area_x.length; i > 0; i--)
            {
                if ((bait_dulj.X == temp_x.value) && (bait_dulj.Y == temp_y.value))

                    return true;

                else
                {
                    temp_x = temp_x.next;
                    temp_y = temp_y.next;
                }
            }

            temp_x = wall_area_x.first;
            temp_y = wall_area_y.first;

            for (int j = wall_area_x.length; j > 0; j--)
            {
                if ((bait_dulj.X == temp_x.value) && (bait_dulj.Y == temp_y.value))
                    return true;
                else
                {
                    temp_x = temp_x.next;
                    temp_y = temp_y.next;
                }
            }

            Node temp_x_2 = snake_area_x_2.first;
            Node temp_y_2 = snake_area_y_2.first;

            for (int i = snake_area_x_2.length; i > 0; i--)
            {
                if ((bait_dulj.X == temp_x_2.value) && (bait_dulj.Y == temp_y_2.value))
                    return true;

                else
                {
                    temp_x_2 = temp_x_2.next;
                    temp_y_2 = temp_y_2.next;

                }
            }

            //ako je na istoj koordinati kao obicna hrana
            if ((bait_dulj.X == bait.X) && (bait_dulj.Y == bait.Y))
                return true;

            //ako je unutar specijalnog podrucja za prelazak zmije preko sebe
            if ((bait_rec.X <= bait_dulj.X) && (bait_rec.X + 6 >= bait_dulj.X)
                    && (bait_rec.Y <= bait_dulj.Y) && (bait_rec.Y + 6 >= bait_dulj.Y))
                return true;

            return false;
        }

        // kontroliranje hrane za podrucje na kojem zmija moze prelaziti preko sebe
        // ako je na istoj koordinati kao i prepreke ili tijelo zmije
        public bool Control_Bait_Coordinate_rec()
        {
            Node temp_x = snake_area_x.first;
            Node temp_y = snake_area_y.first;

            // kako je pravokutnik velicine 5x5 moramo provjeriti za svaku od 5 koordinata da se ne poklapa
            for (int i = snake_area_x.length; i > 0; i--)
            {
                if ((bait_rec.X == temp_x.value) && (bait_rec.Y == temp_y.value))
                    return true;

                if ((bait_rec.X <= temp_x.value) && (bait_rec.X + 6 >= temp_x.value)
                    && (bait_rec.Y <= temp_y.value) && (bait_rec.Y + 6 >= temp_y.value))
                    return true;

                else
                {
                    temp_x = temp_x.next;
                    temp_y = temp_y.next;

                }
            }

            temp_x = wall_area_x.first;
            temp_y = wall_area_y.first;

            // kako je pravokutnik velicine 5x5 moramo provjeriti za svaku od 5 koordinata da se ne poklapa
            for (int j = wall_area_x.length; j > 0; j--)
            {
                if ((bait_rec.X == temp_x.value) && (bait_rec.Y == temp_y.value))
                    return true;

                if ((bait_rec.X <= temp_x.value) && (bait_rec.X + 6 >= temp_x.value)
                    && (bait_rec.Y <= temp_y.value) && (bait_rec.Y + 6 >= temp_y.value))
                    return true;

                else
                {
                    temp_x = temp_x.next;
                    temp_y = temp_y.next;
                }
            }

            // da se ne stavra nad zmijom 2
            Node temp_x_2 = snake_area_x_2.first;
            Node temp_y_2 = snake_area_y_2.first;

            for (int i = snake_area_x_2.length; i > 0; i--)
            {
                if ((bait_rec.X == temp_x_2.value) && (bait_rec.Y == temp_y_2.value))
                    return true;

                if ((bait_rec.X <= temp_x_2.value) && (bait_rec.X + 6 >= temp_x_2.value)
                    && (bait_rec.Y <= temp_y_2.value) && (bait_rec.Y + 6 >= temp_y_2.value))
                    return true;

                else
                {
                    temp_x_2 = temp_x_2.next;
                    temp_y_2 = temp_y_2.next;

                }
            }

            //da se ne postavlja na obicnu hranu
            if ((bait_rec.X <= bait.X) && (bait_rec.X + 6 >= bait.X)
                   && (bait_rec.Y <= bait.Y) && (bait_rec.Y + 6 >= bait.Y))
                return true;

            //da se ne postavlja na spec hranu za povecanje bodova
            if ((bait_rec.X <= bait_inc.X) && (bait_rec.X + 6 >= bait_inc.X)
                   && (bait_rec.Y <= bait_inc.Y) && (bait_rec.Y + 6 >= bait_inc.Y))
                return true;

            //da se ne postavlja na spec hranu za smanjivanje bodova
            if ((bait_rec.X <= bait_dec.X) && (bait_rec.X + 6 >= bait_dec.X)
                   && (bait_rec.Y <= bait_dec.Y) && (bait_rec.Y + 6 >= bait_dec.Y))
                return true;

            //da se ne postavlja na spec hranu za duljinu
            if ((bait_rec.X <= bait_dulj.X) && (bait_rec.X + 6 >= bait_dulj.X)
                   && (bait_rec.Y <= bait_dulj.Y) && (bait_rec.Y + 6 >= bait_dulj.Y))
                return true;

            //da se ne postavlja na spec hranu za prelazak levela
            if ((bait_rec.X <= bait_lv.X) && (bait_rec.X + 6 >= bait_lv.X)
                && (bait_rec.Y <= bait_lv.Y) && (bait_rec.Y + 6 >= bait_lv.Y))
                return true;

            //da se ne postavlja na spec hranu za promjenu smjera
            if ((bait_rec.X <= bait_smjer.X) && (bait_rec.X + 6 >= bait_smjer.X)
                   && (bait_rec.Y <= bait_smjer.Y) && (bait_rec.Y + 6 >= bait_smjer.Y))
                return true;

            return false;
        }



        Random rnd = new Random();
        // odredivanje random coordinata za hranu običnu
        public void Find_Coordinate_For_Bait()
        {
            //Random rnd = new Random();

            bait.X = rnd.Next(1, game_width / node_length - 1);
            bait.Y = rnd.Next(1, game_height / node_length - 1);

            bait_bool = true;

        }

        // odredivanje random coordinata za hranu koja smanjuje bodove
        public void Find_Coordinate_For_Bait_dec()
        {
            //Random rnd = new Random();

            bait_dec.X = rnd.Next(1, game_width / node_length - 1);
            bait_dec.Y = rnd.Next(1, game_height / node_length - 1);

            bait_bool_dec = true;

        }

        // odredivanje random coordinata za hranu koja zavrsava level
        public void Find_Coordinate_For_Bait_lv()
        {
            //Random rnd = new Random();

            bait_lv.X = rnd.Next(1, game_width / node_length - 1);
            bait_lv.Y = rnd.Next(1, game_height / node_length - 1);

            bait_bool_lv = true;

        }


        // odredivanje random coordinata za hranu koja povecava bodove
        public void Find_Coordinate_For_Bait_inc()
        {
            //Random rnd = new Random();

            bait_inc.X = rnd.Next(1, game_width / node_length - 1);
            bait_inc.Y = rnd.Next(1, game_height / node_length - 1);

            bait_bool_inc = true;

        }

        // odredivanje random coordinata za hranu koja mijenja smijer
        public void Find_Coordinate_For_Bait_smjer()
        {
            //Random rnd = new Random();

            bait_smjer.X = rnd.Next(1, game_width / node_length - 1);
            bait_smjer.Y = rnd.Next(1, game_height / node_length - 1);

            bait_bool_smjer = true;

        }

        // odredivanje random coordinata za hranu za duljinu
        public void Find_Coordinate_For_Bait_dulj()
        {
            //Random rnd = new Random();

            bait_dulj.X = rnd.Next(1, game_width / node_length - 1);
            bait_dulj.Y = rnd.Next(1, game_height / node_length - 1);

            bait_bool_dulj = true;

        }

        // odredivanje random coordinata za hranu za podrucje gdje zmija smije prelaziti preko sebe
        public void Find_Coordinate_For_Bait_rec()
        {
            //Random rnd = new Random();

            bait_rec.X = rnd.Next(6, game_width / node_length - 6);
            bait_rec.Y = rnd.Next(6, game_height / node_length - 6);

            bait_bool_rec = true;

        }




        // stavljanje obične hrane na koordinatu
        public void Add_New_Bait(int x, int y)
        {
            if (!Control_Bait_Coordinate())
            {
                graph.FillRectangle(bait_skin_color, x * node_length, y * node_length, node_length, node_length);
                graph.FillRectangle(bait_body_color, x * node_length, y * node_length, node_length - node_border_length, node_length - node_border_length);
            }

            else bait_bool = false;
        }

        // stavljanje hrane za povecavanje bodova na koordinatu
        public void Add_New_Bait_inc(int x, int y)
        {
            if (!Control_Bait_Coordinate_inc())
            { //može se crtat i neki drugi oblik                
                graph.FillRectangle(bait_skin_color_inc, x * node_length, y * node_length, node_length, node_length);
                graph.FillRectangle(bait_body_color_inc, x * node_length, y * node_length, node_length - node_border_length, node_length - node_border_length);

                incPostavljen = 1;
            }

            else bait_bool_inc = false;

        }

        // stavljanje hrane za smanjivanje bodova na koordinatu
        public void Add_New_Bait_dec(int x, int y)
        {
            if (!Control_Bait_Coordinate_dec())
            { //može se crtat i neki drugi oblik

                graph.FillRectangle(bait_skin_color_dec, x * node_length, y * node_length, node_length, node_length);
                graph.FillRectangle(bait_body_color_dec, x * node_length, y * node_length, node_length - node_border_length, node_length - node_border_length);

                decPostavljen = 1;
            }

            else bait_bool_dec = false;

        }

        // stavljanje hrane za zavrsetak levela na koordinatu
        public void Add_New_Bait_lv(int x, int y)
        {
            if (!Control_Bait_Coordinate_lv())
            {
                graph.FillRectangle(bait_skin_color_lv, x * node_length, y * node_length, node_length, node_length);
                graph.FillRectangle(bait_body_color_lv, x * node_length, y * node_length, node_length - node_border_length, node_length - node_border_length);

                lvPostavljen = 1;
            }

            else bait_bool_lv = false;
        }

        // stavljanje hrane za promjenu smijera na koordinatu
        public void Add_New_Bait_smjer(int x, int y)
        {
            if (!Control_Bait_Coordinate_smjer())
            {
                graph.FillRectangle(bait_skin_color_smjer, x * node_length, y * node_length, node_length, node_length);
                graph.FillRectangle(bait_body_color_smjer, x * node_length, y * node_length, node_length - node_border_length, node_length - node_border_length);

                smjerPostavljen = 1;
            }

            else bait_bool_smjer = false;
        }

        // stavljanje hrane za duljinu na koordinatu
        public void Add_New_Bait_dulj(int x, int y)
        {
            if (!Control_Bait_Coordinate_dulj())
            {
                graph.FillRectangle(bait_skin_color_dulj, x * node_length, y * node_length, node_length, node_length);
                graph.FillRectangle(bait_body_color_dulj, x * node_length, y * node_length, node_length - node_border_length, node_length - node_border_length);

                duljPostavljen = 1;
            }

            else bait_bool_dulj = false;
        }


        // stavljanje pravokutnika gdje ce zmija smjeti prelaziti preko sebe
        public void Add_New_Rectangle(int x, int y)
        {
            if (!Control_Bait_Coordinate_rec())
            {
                graph.FillRectangle(rectangle_color, x * node_length, y * node_length, node_length * 5, node_length * 5);
            }
            else bait_bool_rec = false;
        }




        // brisanje hrane za smanjivanje bodova na odredenoj koordinati
        public void Delete_bait_dec(int x, int y)
        {

            graph.FillRectangle(back_color, x * node_length, y * node_length, node_length, node_length);
            graph.FillRectangle(back_color, x * node_length, y * node_length, node_length - node_border_length, node_length - node_border_length);

        }

        // brisanje hrane za dodavanje bodova na odredenoj koordinati
        public void Delete_bait_inc(int x, int y)
        {

            graph.FillRectangle(back_color, x * node_length, y * node_length, node_length, node_length);
            graph.FillRectangle(back_color, x * node_length, y * node_length, node_length - node_border_length, node_length - node_border_length);

        }

        // brisanje hrane za zavrsetak levela na odredenoj koordinati
        public void Delete_bait_lv(int x, int y)
        {

            graph.FillRectangle(back_color, x * node_length, y * node_length, node_length, node_length);
            graph.FillRectangle(back_color, x * node_length, y * node_length, node_length - node_border_length, node_length - node_border_length);

        }

        // brisanje hrane za duljinu na odredenoj koordinati
        public void Delete_bait_dulj(int x, int y)
        {

            graph.FillRectangle(back_color, x * node_length, y * node_length, node_length, node_length);
            graph.FillRectangle(back_color, x * node_length, y * node_length, node_length - node_border_length, node_length - node_border_length);

        }

        // brisanje hrane za promjenu smjera levela na odredenoj koordinati
        public void Delete_bait_smjer(int x, int y)
        {

            graph.FillRectangle(back_color, x * node_length, y * node_length, node_length, node_length);
            graph.FillRectangle(back_color, x * node_length, y * node_length, node_length - node_border_length, node_length - node_border_length);

        }




        // provjera jeli zmija pojela hranu običnu
        public bool Did_Snake_Eat_The_Bait()
        {
            if ((node_x == bait.X) && (node_y == bait.Y))
                return true;
            else
                return false;
        }

        public bool Did_Snake_Eat_The_Bait_2()
        {
            if ((node_x_2 == bait.X) && (node_y_2 == bait.Y))
                return true;
            else
                return false;
        }

        // provjera jeli zmija pojela hranu za dodavanje bodova
        public bool Did_Snake_Eat_The_Bait_inc()
        {
            if ((node_x == bait_inc.X) && (node_y == bait_inc.Y))
                return true;
            else
                return false;
        }

        // provjera jeli zmija pojela hranu za zavrsetak levela
        public bool Did_Snake_Eat_The_Bait_lv()
        {
            if ((node_x == bait_lv.X) && (node_y == bait_lv.Y))
                return true;
            else
                return false;
        }

        // provjera jeli zmija pojela hranu za smanjivanje bodova
        public bool Did_Snake_Eat_The_Bait_dec()
        {
            if ((node_x == bait_dec.X) && (node_y == bait_dec.Y))
                return true;
            else
                return false;
        }

        // provjera jeli zmija pojela hranu za promijenu smjera
        public bool Did_Snake_Eat_The_Bait_smjer()
        {
            if ((node_x == bait_smjer.X) && (node_y == bait_smjer.Y))
                return true;
            else
                return false;
        }

        // provjera jeli zmija pojela hranu za produljivanje
        public bool Did_Snake_Eat_The_Bait_dulj()
        {
            if ((node_x == bait_dulj.X) && (node_y == bait_dulj.Y))
                return true;
            else
                return false;
        }



        // provjera jeli se zmija sudarila sama sa sobom
        public bool Did_Snake_Crash_Byself(int temp_route)
        {
            int x = node_x * node_length;
            int y = node_y * node_length;
            try
            {
                if (temp_route == 4)
                {
                    if (bmp.GetPixel(x + node_length / 2, y + node_length / 2) == bmp.GetPixel(x + node_length / 2 - node_length, y + node_length / 2))
                    {
                        return true;
                    }

                    if (bmp.GetPixel(x + node_length / 2 - node_length, y + node_length / 2) == new Pen(wall_body_color).Color)
                    {
                        return true;
                    }

                    if (bmp.GetPixel(x + node_length / 2 - node_length, y + node_length / 2) == new Pen(snake_body_color_2).Color)
                    {
                        return true;
                    }
                }

                if (temp_route == 2)
                {
                    if (bmp.GetPixel(x + node_length / 2, y + node_length / 2) == bmp.GetPixel(x + node_length / 2 + node_length, y + node_length / 2))
                    {
                        return true;
                    }

                    if (bmp.GetPixel(x + node_length / 2 + node_length, y + node_length / 2) == new Pen(wall_body_color).Color)
                    {
                        return true;
                    }

                    if (bmp.GetPixel(x + node_length / 2 + node_length, y + node_length / 2) == new Pen(snake_body_color_2).Color)
                    {
                        return true;
                    }
                }

                if (temp_route == 1)
                {
                    if (bmp.GetPixel(x + node_length / 2, y + node_length / 2) == bmp.GetPixel(x + node_length / 2, y + node_length / 2 - node_length))
                    {
                        return true;
                    }

                    if (bmp.GetPixel(x + node_length / 2, y + node_length / 2 - node_length) == new Pen(wall_body_color).Color)
                    {
                        return true;
                    }

                    if (bmp.GetPixel(x + node_length / 2, y + node_length / 2 - node_length) == new Pen(snake_body_color_2).Color)
                    {
                        return true;
                    }
                }

                if (temp_route == 3)
                {
                    if (bmp.GetPixel(x + node_length / 2, y + node_length / 2) == bmp.GetPixel(x + node_length / 2, y + node_length / 2 + node_length))
                    {
                        return true;
                    }

                    if (bmp.GetPixel(x + node_length / 2, y + node_length / 2 + node_length) == new Pen(wall_body_color).Color)
                    {
                        return true;
                    }

                    if (bmp.GetPixel(x + node_length / 2, y + node_length / 2 + node_length) == new Pen(snake_body_color_2).Color)
                    {
                        return true;
                    }
                }

            }
            catch (Exception e) { string str = e.Message.ToString(); return false; }

            return false;
        }

        // naredeno da gleda samo jel se sudarila sa igracevom zmijom
        public bool Did_Snake_Crash_Byself_2(int temp_route_2)
        {
            int x = node_x_2 * node_length;
            int y = node_y_2 * node_length;
            try
            {
                if (temp_route_2 == 4)
                {
                    if (bmp.GetPixel(x + node_length / 2 - node_length, y + node_length / 2) == new Pen(snake_body_color).Color)
                    {
                        return true;
                    }
                }

                if (temp_route_2 == 2)
                {
                    if (bmp.GetPixel(x + node_length / 2 + node_length, y + node_length / 2) == new Pen(snake_body_color).Color)
                    {
                        return true;
                    }
                }

                if (temp_route_2 == 1)
                {
                    if (bmp.GetPixel(x + node_length / 2, y + node_length / 2 - node_length) == new Pen(snake_body_color).Color)
                    {
                        return true;
                    }
                }

                if (temp_route_2 == 3)
                {
                    if (bmp.GetPixel(x + node_length / 2, y + node_length / 2 + node_length) == new Pen(snake_body_color).Color)
                    {
                        return true;
                    }
                }

            }
            catch (Exception e) { string str = e.Message.ToString(); return false; }

            return false;
        }



        // stavljanje cvora za prepreku na odredenu koordinatu
        public void Add_Wall_Node(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2)
            {
                while (y1 <= y2)
                {
                    graph.FillRectangle(wall_skin_color, x1 * node_length, y1 * node_length, node_length, node_length);
                    graph.FillRectangle(wall_body_color, x1 * node_length, y1 * node_length, node_length - node_border_length, node_length - node_border_length);
                    wall_area_x.Add_Node(x1);
                    wall_area_y.Add_Node(y1);
                    y1++;
                }
            }

            else if (y1 == y2)
            {
                while (x1 <= x2)
                {
                    graph.FillRectangle(wall_skin_color, x1 * node_length, y1 * node_length, node_length, node_length);
                    graph.FillRectangle(wall_body_color, x1 * node_length, y1 * node_length, node_length - node_border_length, node_length - node_border_length);
                    wall_area_x.Add_Node(x1);
                    wall_area_y.Add_Node(y1);
                    x1++;
                }
            }
        }


        // reproduciranje zvuka kada zmija pojede hranu
        public void Eated_Sound()
        {
            if (sound_control == "on" && File.Exists(sound_eated.SoundLocation))
                sound_eated.Play();
        }


        // reproduciranje zvuka kada je igrač u menu-u
        public void Main_Menu_Sound()
        {
            if (sound_control == "on" && File.Exists(sound_eated.SoundLocation))
                sound_menu.PlayLooping(); 

        }

        // reproduciranje zvuka kada je igra gotova
        public void Gameover_Sound()
        {
            if (sound_control == "on" && File.Exists(sound_eated.SoundLocation))
                sound_gameover.PlayLooping(); 
        }


        // dizajn menu-a
        public void Main_Menu(int selected)
        {

            menu_bmp = new Bitmap(game_width, game_height);
            menu_graph = Graphics.FromImage(menu_bmp);
            menu_graph.FillRectangle(back_color, 0, 0, game_width, game_height);


            Font arial3 = new Font(fontfamily, 70, FontStyle.Regular);
            Font arial2 = new Font(fontfamily, 20, FontStyle.Regular);
            menu_graph.DrawString("ZMIJA", arial3, snake_skin_color, game_width / 4, game_height / 4);

            if (selected == 1)
            {
                menu_graph.DrawString(">  ZAPOCNI IGRU", arial2, Brushes.FloralWhite, game_width / 4 + 100, game_height / 4 + 150);
                menu_graph.DrawString("   NAREDBE", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 190);
                menu_graph.DrawString("   POSTAVKE", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 230);
                menu_graph.DrawString("   O NAMA", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 270);
                menu_graph.DrawString("   IZLAZ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 310);

            }
            else if (selected == 2)
            {
                menu_graph.DrawString("   ZAPOCNI IGRU", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 150);
                menu_graph.DrawString(">  NAREDBE", arial2, Brushes.FloralWhite, game_width / 4 + 100, game_height / 4 + 190);
                menu_graph.DrawString("   POSTAVKE", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 230);
                menu_graph.DrawString("   O NAMA", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 270);
                menu_graph.DrawString("   IZLAZ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 310);

            }
            else if (selected == 3)
            {
                menu_graph.DrawString("   ZAPOCNI IGRU", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 150);
                menu_graph.DrawString("   NAREDBE", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 190);
                menu_graph.DrawString(">  POSTAVKE", arial2, Brushes.FloralWhite, game_width / 4 + 100, game_height / 4 + 230);
                menu_graph.DrawString("   O NAMA", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 270);
                menu_graph.DrawString("   IZLAZ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 310);

            }
            else if (selected == 4)
            {
                menu_graph.DrawString("   ZAPOCNI IGRU", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 150);
                menu_graph.DrawString("   NAREDBE", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 190);
                menu_graph.DrawString("   POSTAVKE", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 230);
                menu_graph.DrawString(">  O NAMA", arial2, Brushes.FloralWhite, game_width / 4 + 100, game_height / 4 + 270);
                menu_graph.DrawString("   IZLAZ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 310);

            }
            else if (selected == 5)
            {
                menu_graph.DrawString("   ZAPOCNI IGRU", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 150);
                menu_graph.DrawString("   NAREDBE", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 190);
                menu_graph.DrawString("   POSTAVKE", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 230);
                menu_graph.DrawString("   O NAMA", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 270);
                menu_graph.DrawString(">  IZLAZ", arial2, Brushes.FloralWhite, game_width / 4 + 100, game_height / 4 + 310);
            }

            menu_graph.DrawString("ODABERI >", arial2, Brushes.FloralWhite, game_width - 200, game_height - 40);
            menu_graph.DrawImage(new Bitmap(Application.StartupPath + "\\pictures\\select.bmp"), game_width - 250, game_height - 40);


        }

        // dizajn za NAREDBE
        public void Instructions()
        {
            int x = game_width;
            int y = game_height;
            menu_bmp = new Bitmap(game_width, game_height);
            menu_graph = Graphics.FromImage(menu_bmp);
            menu_graph.FillRectangle(back_color, 0, 0, game_width, game_height);

            Font arial2 = new Font(fontfamily, 20, FontStyle.Regular);
            Font arial3 = new Font(fontfamily, 70, FontStyle.Regular);

            menu_graph.DrawString("ZMIJA", arial3, snake_skin_color, x / 4, y / 4);

            x = x / 4 + x / 10;
            y = y / 2 - y / 11;
            menu_graph.DrawString(">   NAREDBE", arial2, snake_body_color, x, y);

            y += game_height / 8;
            menu_graph.DrawImage(new Bitmap(Application.StartupPath + "\\pictures\\up.bmp"), x, y);
            menu_graph.DrawString("   GORE", arial2, Brushes.FloralWhite, x, y);

            y += y / 12;
            menu_graph.DrawImage(new Bitmap(Application.StartupPath + "\\pictures\\right.bmp"), x, y);
            menu_graph.DrawString("   DESNO", arial2, Brushes.FloralWhite, x, y);

            y += y / 12;
            menu_graph.DrawImage(new Bitmap(Application.StartupPath + "\\pictures\\down.bmp"), x, y);
            menu_graph.DrawString("   DOLJE", arial2, Brushes.FloralWhite, x, y);

            y += y / 12;
            menu_graph.DrawImage(new Bitmap(Application.StartupPath + "\\pictures\\left.bmp"), x, y);
            menu_graph.DrawString("   LIJEVO", arial2, Brushes.FloralWhite, x, y);

            y += y / 12;
            menu_graph.DrawImage(new Bitmap(Application.StartupPath + "\\pictures\\select.bmp"), x, y);
            menu_graph.DrawString("   ODABERI", arial2, Brushes.FloralWhite, x, y);

            y += y / 12;
            menu_graph.DrawImage(new Bitmap(Application.StartupPath + "\\pictures\\back.bmp"), x, y);
            menu_graph.DrawString("   NAZAD", arial2, Brushes.FloralWhite, x, y);



            menu_graph.DrawString("< NAZAD", arial2, Brushes.FloralWhite, 10, game_height - 40);
            menu_graph.DrawImage(new Bitmap(Application.StartupPath + "\\pictures\\back.bmp"), 200, game_height - 40);

        }


        // dizajn za O NAMA
        public void About()
        {

            int x = game_width;
            int y = game_height;
            menu_bmp = new Bitmap(game_width, game_height);
            menu_graph = Graphics.FromImage(menu_bmp);
            menu_graph.FillRectangle(back_color, 0, 0, game_width, game_height);

            Font arial2 = new Font(fontfamily, 20, FontStyle.Regular);
            Font arial3 = new Font(fontfamily, 70, FontStyle.Regular);

            menu_graph.DrawString("ZMIJA", arial3, snake_skin_color, x / 4, y / 4);

            x = x / 4 + x / 10;
            y = y / 2 - y / 11;
            menu_graph.DrawString(">   O NAMA", arial2, snake_body_color, x, y);

            y += game_height / 8;
            x -= x / 5;
            menu_graph.DrawString("Zavrsni projekt iz RP3\n\n" +
                                   "Andrej Pavlović\n" +
                                   "Igor Sušić\n" +
                                   "Jure Šiljeg\n" +
                                   "\n" +
                                   "Ak.g. 2014/2015", arial2, Brushes.FloralWhite, x, y - 10);



            menu_graph.DrawString("< NAZAD", arial2, Brushes.FloralWhite, 10, game_height - 40);
            menu_graph.DrawImage(new Bitmap(Application.StartupPath + "\\pictures\\back.bmp"), 200, game_height - 40);

        }

        // dizajn za POSTAVKE
        public void Settings(int selected)
        {
            menu_bmp = new Bitmap(game_width, game_height);
            menu_graph = Graphics.FromImage(menu_bmp);
            menu_graph.FillRectangle(back_color, 0, 0, game_width, game_height);

            Font arial3 = new Font(fontfamily, 70, FontStyle.Regular);
            Font arial2 = new Font(fontfamily, 20, FontStyle.Regular);
            menu_graph.DrawString("ZMIJA", arial3, snake_skin_color, game_width / 4, game_height / 4);

            if (selected == 1) // ZVUK
            {
                menu_graph.DrawString(">  ZVUK    " + sound_control, arial2, Brushes.FloralWhite, game_width / 4 + 10, game_height / 4 + 150);
                menu_graph.DrawString("   PRIKAZ  " + game_width.ToString() + " : " + game_height.ToString(), arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 190);
                menu_graph.DrawString("   TEZINA  " + speed_control, arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 230);
                menu_graph.DrawString("   KOMBINACIJE TIPKI  :", arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 270);
                menu_graph.DrawString("   Gore:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 310);
                menu_graph.DrawString("   ALT  " + tipkaZaGore, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 310);
                menu_graph.DrawString("   Dolje:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 350);
                menu_graph.DrawString("   ALT  " + tipkaZaDolje, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 350);
                menu_graph.DrawString("   Lijevo:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 390);
                menu_graph.DrawString("   ALT  " + tipkaZaLijevo, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 390);
                menu_graph.DrawString("   Desno:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 430);
                menu_graph.DrawString("   ALT  " + tipkaZaDesno, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 430);
            }

            if (selected == 2) // PRIKAZ
            {
                menu_graph.DrawString("   ZVUK    " + sound_control, arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 150);
                menu_graph.DrawString(">  PRIKAZ  " + game_width.ToString() + " : " + game_height.ToString(), arial2, Brushes.FloralWhite, game_width / 4 + 10, game_height / 4 + 190);
                menu_graph.DrawString("   TEZINA  " + speed_control, arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 230);
                menu_graph.DrawString("   KOMBINACIJE TIPKI  :", arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 270);
                menu_graph.DrawString("   Gore:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 310);
                menu_graph.DrawString("   ALT  " + tipkaZaGore, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 310);
                menu_graph.DrawString("   Dolje:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 350);
                menu_graph.DrawString("   ALT  " + tipkaZaDolje, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 350);
                menu_graph.DrawString("   Lijevo:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 390);
                menu_graph.DrawString("   ALT  " + tipkaZaLijevo, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 390);
                menu_graph.DrawString("   Desno:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 430);
                menu_graph.DrawString("   ALT  " + tipkaZaDesno, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 430);
            }

            if (selected == 3) // TEZINA
            {
                menu_graph.DrawString("   ZVUK    " + sound_control, arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 150);
                menu_graph.DrawString("   PRIKAZ  " + game_width.ToString() + " : " + game_height.ToString(), arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 190);
                menu_graph.DrawString(">   TEZINA  " + speed_control, arial2, Brushes.FloralWhite, game_width / 4 + 10, game_height / 4 + 230);
                menu_graph.DrawString("   KOMBINACIJE TIPKI  :", arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 270);
                menu_graph.DrawString("   Gore:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 310);
                menu_graph.DrawString("   ALT  " + tipkaZaGore, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 310);
                menu_graph.DrawString("   Dolje:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 350);
                menu_graph.DrawString("   ALT  " + tipkaZaDolje, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 350);
                menu_graph.DrawString("   Lijevo:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 390);
                menu_graph.DrawString("   ALT  " + tipkaZaLijevo, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 390);
                menu_graph.DrawString("   Desno:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 430);
                menu_graph.DrawString("   ALT  " + tipkaZaDesno, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 430);
            }

            if (selected == 4) // tipka za gore
            {
                menu_graph.DrawString("   ZVUK    " + sound_control, arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 150);
                menu_graph.DrawString("   PRIKAZ  " + game_width.ToString() + " : " + game_height.ToString(), arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 190);
                menu_graph.DrawString("   TEZINA  " + speed_control, arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 230);
                menu_graph.DrawString("   KOMBINACIJE TIPKI  :", arial2, Brushes.FloralWhite, game_width / 4 + 10, game_height / 4 + 270);
                menu_graph.DrawString("   Gore:  ", arial2, Brushes.FloralWhite, game_width / 4 + 100, game_height / 4 + 310);
                menu_graph.DrawString(">   ALT  " + tipkaZaGore, arial2, Brushes.FloralWhite, game_width / 4 + 300, game_height / 4 + 310);
                menu_graph.DrawString("   Dolje:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 350);
                menu_graph.DrawString("   ALT  " + tipkaZaDolje, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 350);
                menu_graph.DrawString("   Lijevo:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 390);
                menu_graph.DrawString("   ALT  " + tipkaZaLijevo, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 390);
                menu_graph.DrawString("   Desno:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 430);
                menu_graph.DrawString("   ALT  " + tipkaZaDesno, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 430);
            }

            if (selected == 5) // tipka za dolje
            {
                menu_graph.DrawString("   ZVUK    " + sound_control, arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 150);
                menu_graph.DrawString("   PRIKAZ  " + game_width.ToString() + " : " + game_height.ToString(), arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 190);
                menu_graph.DrawString("   TEZINA  " + speed_control, arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 230);
                menu_graph.DrawString("   KOMBINACIJE TIPKI  :", arial2, Brushes.FloralWhite, game_width / 4 + 10, game_height / 4 + 270);
                menu_graph.DrawString("   Gore:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 310);
                menu_graph.DrawString("   ALT  " + tipkaZaGore, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 310);
                menu_graph.DrawString("   Dolje:  ", arial2, Brushes.FloralWhite, game_width / 4 + 100, game_height / 4 + 350);
                menu_graph.DrawString(">   ALT  " + tipkaZaDolje, arial2, Brushes.FloralWhite, game_width / 4 + 300, game_height / 4 + 350);
                menu_graph.DrawString("   Lijevo:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 390);
                menu_graph.DrawString("   ALT  " + tipkaZaLijevo, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 390);
                menu_graph.DrawString("   Desno:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 430);
                menu_graph.DrawString("   ALT  " + tipkaZaDesno, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 430);
            }

            if (selected == 6) // tipka za lijevo
            {
                menu_graph.DrawString("   ZVUK    " + sound_control, arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 150);
                menu_graph.DrawString("   PRIKAZ  " + game_width.ToString() + " : " + game_height.ToString(), arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 190);
                menu_graph.DrawString("   TEZINA  " + speed_control, arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 230);
                menu_graph.DrawString("   KOMBINACIJE TIPKI  :", arial2, Brushes.FloralWhite, game_width / 4 + 10, game_height / 4 + 270);
                menu_graph.DrawString("   Gore:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 310);
                menu_graph.DrawString("   ALT  " + tipkaZaGore, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 310);
                menu_graph.DrawString("   Dolje:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 350);
                menu_graph.DrawString("   ALT  " + tipkaZaDolje, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 350);
                menu_graph.DrawString("   Lijevo:  ", arial2, Brushes.FloralWhite, game_width / 4 + 100, game_height / 4 + 390);
                menu_graph.DrawString(">   ALT  " + tipkaZaLijevo, arial2, Brushes.FloralWhite, game_width / 4 + 300, game_height / 4 + 390);
                menu_graph.DrawString("   Desno:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 430);
                menu_graph.DrawString("   ALT  " + tipkaZaDesno, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 430);
            }

            if (selected == 7) // tipka za desno
            {
                menu_graph.DrawString("   ZVUK    " + sound_control, arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 150);
                menu_graph.DrawString("   PRIKAZ  " + game_width.ToString() + " : " + game_height.ToString(), arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 190);
                menu_graph.DrawString("   TEZINA  " + speed_control, arial2, snake_body_color, game_width / 4 + 10, game_height / 4 + 230);
                menu_graph.DrawString("   KOMBINACIJE TIPKI  :", arial2, Brushes.FloralWhite, game_width / 4 + 10, game_height / 4 + 270);
                menu_graph.DrawString("   Gore:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 310);
                menu_graph.DrawString("   ALT  " + tipkaZaGore, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 310);
                menu_graph.DrawString("   Dolje:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 350);
                menu_graph.DrawString("   ALT  " + tipkaZaDolje, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 350);
                menu_graph.DrawString("   Lijevo:  ", arial2, snake_body_color, game_width / 4 + 100, game_height / 4 + 390);
                menu_graph.DrawString("   ALT  " + tipkaZaLijevo, arial2, snake_body_color, game_width / 4 + 300, game_height / 4 + 390);
                menu_graph.DrawString("   Desno:  ", arial2, Brushes.FloralWhite, game_width / 4 + 100, game_height / 4 + 430);
                menu_graph.DrawString(">   ALT  " + tipkaZaDesno, arial2, Brushes.FloralWhite, game_width / 4 + 300, game_height / 4 + 430);
            }




            menu_graph.DrawString("PROMIJENI >", arial2, Brushes.FloralWhite, game_width - 250, game_height - 40);
            menu_graph.DrawImage(new Bitmap(Application.StartupPath + "\\pictures\\select.bmp"), game_width - 300, game_height - 40);


            menu_graph.DrawString("< NAZAD", arial2, Brushes.FloralWhite, 10, game_height - 40);
            menu_graph.DrawImage(new Bitmap(Application.StartupPath + "\\pictures\\back.bmp"), 200, game_height - 40);
        }

        // dizajn za PAUZU
        public void Pause()
        {
            menu_bmp = new Bitmap(game_width, game_height);
            menu_graph = Graphics.FromImage(menu_bmp);
            menu_graph.FillRectangle(back_color, 0, 0, game_width, game_height);

            Font arial2 = new Font(fontfamily, 20, FontStyle.Regular);

            menu_graph.DrawString("PAUZA  " + brojIskorPauza + "/" + brojDozPauza,
                                   arial2, snake_skin_color, game_width / 2 - 150, game_height / 2);

            brojIskorPauza++;
        }


        // dizaj na prozor koji se prikazuje kada igrac zavrsi level
        public void Clear_Level()
        {
            Font arial2 = new Font(fontfamily, 20, FontStyle.Regular);

            graph.FillRectangle(bait_body_color, game_width / 3, game_height / 3, 380, game_height / 3);
            graph.DrawString("    CESTITKE\n Level-" + game_level.ToString() + " ZAVRSEN\n\nENTER ZA Level-" + (game_level + 1).ToString(), arial2, Brushes.Thistle, game_width / 3 + 25, game_height / 3 + game_height / 20);

        }


        // dizajn za prozor koji se prikazuje kada igrac zavrsi sve levele tj. igru
        public void Finish_Game()
        {
            Font arial2 = new Font(fontfamily, 20, FontStyle.Regular);

            graph.FillRectangle(bait_body_color, game_width / 3, game_height / 3, 380, game_height / 3);
            graph.DrawString("   CESTITKE !!", arial2, Brushes.Thistle, game_width / 3 + 25, game_height / 3 + game_height / 20);
            graph.DrawString("    KRAJ IGRE!", arial2, Brushes.Thistle, game_width / 3 + 5, game_height / 3 + game_height / 8);
        }


        // design a page that is displayed when snake crashed- game over.
        public void Game_Over()
        {
            Font arial2 = new Font(fontfamily, 20, FontStyle.Regular);

            graph.FillRectangle(bait_body_color, game_width / 3, game_height / 3, 380, game_height / 4);

            if (brojIskorZivota < brojDozZivota)
            {
                graph.DrawString("  ZIVOTI  " + brojIskorZivota + "/" + brojDozZivota, arial2, Brushes.Thistle, game_width / 3 + 25, game_height / 3 + game_height / 20);
                graph.DrawString("ENTER ZA NASTAVAK", arial2, Brushes.Thistle, game_width / 3 + 5, game_height / 3 + game_height / 8);
            }
            else
            {
                graph.DrawString("   GAME OVER", arial2, Brushes.Thistle, game_width / 3 + 25, game_height / 3 + game_height / 20);
                graph.DrawString("ENTER ZA NASTAVAK", arial2, Brushes.Thistle, game_width / 3 + 5, game_height / 3 + game_height / 8);
                Gameover_Sound();
            }

            brojIskorZivota++;
        }
    }

}
