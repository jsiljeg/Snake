using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using System.Threading;
using Microsoft.DirectX.DirectInput;

namespace Snake
{
    public partial class MyGame : Form
    {
        public MyGame()
        {
            InitializeComponent();


            // postavljanje pocetne velicine prozora
            this.Size = new Size(1250, 740);

            mygame.game_height = (this.Size.Height - 50) - ((this.Size.Height - 50) % mygame.node_length);
            mygame.game_width = (this.Size.Width - 50) - ((this.Size.Width - 50) % mygame.node_length);

            int h = mygame.game_height;
            int w = mygame.game_width;
            mygame = new Game(h, w);
            top_menu_panel.Size = new Size(w, top_menu_panel.Size.Height);
        }


        public Game mygame = new Game(740, 760);


        //ucitavanje glavne forme
        void MyGame_Form_Load(object sender, EventArgs e)
        {
            lb_point.Font = new Font(mygame.fontfamily, 12, FontStyle.Regular);
            lb_level.Font = new Font(mygame.fontfamily, 12, FontStyle.Regular);
            Level_Design();
            Get_Ready();
            Get_Ready_2();
            mygame.Main_Menu(1);
            pictureBox1.Image = mygame.menu_bmp;
            mygame.Main_Menu_Sound();

        }


        // priprema za startanje igre, doddavanje cvorova za startanje
        void Get_Ready()
        {
            //prvo pocistimo staru zmiju ako slucajno postojim
            // treba nam npr. zbog novog zivota da se izbrise zmija iz prethodnog
            Node temp_x = mygame.snake_area_x.first;
            Node temp_y = mygame.snake_area_y.first;

            if (mygame.snake_area_x.length > 0)
            {
                for (int j = mygame.snake_area_x.length; j > 0; j--)
                {
                    mygame.snake_area_x.Delete_First_Node();
                }
            }
            if (mygame.snake_area_y.length > 0)
            {
                for (int j = mygame.snake_area_y.length; j > 0; j--)
                {
                    mygame.snake_area_y.Delete_First_Node();
                }
            }

            // sada izgradimo novu zmiju
            int i = mygame.snake_length;
            while (i > 0)
            {
                mygame.snake_area_x.Add_Node(mygame.node_x);
                mygame.snake_area_y.Add_Node(mygame.node_y);
                mygame.Add_Snake_Node(mygame.node_x, mygame.node_y);
                i--;
                mygame.node_x++;
            }
            mygame.node_x--;
        }

        void Get_Ready_2()
        {
            // drugu zmiju dodamo u 6. levelu
            if (mygame.game_level == 6)
            {
                //prvo pocistimo staru zmiju ako slucajno postojim
                // treba nam npr. zbog novog zivota da se izbrise zmija iz prethodnog
                Node temp_x_2 = mygame.snake_area_x_2.first;
                Node temp_y_2 = mygame.snake_area_y_2.first;

                if (mygame.snake_area_x_2.length > 0)
                {
                    for (int j = mygame.snake_area_x_2.length; j > 0; j--)
                    {
                        mygame.snake_area_x_2.Delete_First_Node();
                    }
                }
                if (mygame.snake_area_y_2.length > 0)
                {
                    for (int j = mygame.snake_area_y_2.length; j > 0; j--)
                    {
                        mygame.snake_area_y_2.Delete_First_Node();
                    }
                }

                // sada izgradimo novu zmiju
                int i = mygame.snake_length_2;
                while (i > 0)
                {
                    mygame.snake_area_x_2.Add_Node(mygame.node_x_2);
                    mygame.snake_area_y_2.Add_Node(mygame.node_y_2);
                    mygame.Add_Snake_Node_2(mygame.node_x_2, mygame.node_y_2);
                    i--;
                    mygame.node_x_2++;
                }
                mygame.node_x_2--;
            }
        }

        // timer za MyGame (pomocu njega se zmija krece)
        void MyGame_TimerTick(object sender, EventArgs e)
        {
            // ako su dvije zmije na igracoj ploci trebaju se kretat naizmjenicno
            if (mygame.game_level == 6)
            {
                if (mygame.br % 2 == 0)
                {
                    Game_Playing();
                }
                else
                {
                    Game_Playing_2();
                }

                if (mygame.br >= 10000)
                    mygame.br = 0;

                mygame.br++;
            }
            // inace se krece samo zmija kojom upravlja igrac
            else
            {
                Game_Playing();
            }

        }


        // glavna funkcija za igru
        // sve kontrole i kretanje zmije 2 je implementirano ovdje
        public bool Game_Playing_2()
        {

            AStarExample.Pathfind(new NodePosition((int)(mygame.node_x_2), (int)(mygame.node_y_2)),
                new NodePosition((int)(mygame.bait.X), (int)(mygame.bait.Y)), mygame.pathfinder);

            mygame.brojacKoord = 1;

            mygame.pathfinder.temp_x_zmija = mygame.node_x_2;
            mygame.pathfinder.temp_y_zmija = mygame.node_y_2;


            if (mygame.Did_Snake_Crash_Byself_2(mygame.snake_route_2)) // provjera jeli se zmija sudarila sa zidom ili sama sa sobom
            {
                // ako je na podrucju na kojem je dozvoljeno da zmija prelazi preko sebe
                if ((mygame.snake_area_x_2.last.value >= mygame.bait_rec.X)
                    && (mygame.snake_area_x_2.last.value <= (mygame.bait_rec.X + 4))
                    && (mygame.bait_rec.X != -1)
                    && (mygame.snake_area_y_2.last.value >= mygame.bait_rec.Y)
                    && (mygame.snake_area_y_2.last.value <= (mygame.bait_rec.Y + 4))
                    && (mygame.bait_rec.Y != -1))
                {
                    MyGame_timer.Enabled = true;
                    wait_timer.Enabled = false;



                    if (mygame.snake_route_2 == 1) mygame.Move_Snake_Up_2();
                    else if (mygame.snake_route_2 == 2) mygame.Move_Snake_Right_2();
                    else if (mygame.snake_route_2 == 3) mygame.Move_Snake_Down_2();
                    else if (mygame.snake_route_2 == 4) mygame.Move_Snake_Left_2();

                    return true;
                }
                //inace
                else
                {
                    MyGame_timer.Enabled = false;
                    wait_timer.Enabled = true;

                    // da ne prekine igru ako se AI zmija sudari sama sa sobom
                    //MyGame_timer.Enabled = true;
                    //wait_timer.Enabled = false;
                    /*
                    // da se nastavai kretati preko sebe ako se sudari sama sa sobom
                    if (mygame.snake_route_2 == 1) mygame.Move_Snake_Up_2();
                    else if (mygame.snake_route_2 == 2) mygame.Move_Snake_Right_2();
                    else if (mygame.snake_route_2 == 3) mygame.Move_Snake_Down_2();
                    else if (mygame.snake_route_2 == 4) mygame.Move_Snake_Left_2();
                    */

                    mygame.wait_time = 20;
                    return true;
                }

            }

            try
            {
                //  if (mygame.brojacKoord >= 1 && AStarExample.pathfindSucceeded == true)
                //{         

                // provjera jel iduci cvor koji je odreden za kretanje jednak cvoru zida
                bool NaPregradiX = false;
                bool NaPregradiY = false;

                Node temp_x = mygame.wall_area_x.first;
                Node temp_y = mygame.wall_area_y.first;

                for(int i =  mygame.wall_area_x.length; i>0; i--)
                {
                    if (AStarExample.koordXi[mygame.brojacKoord] == temp_x.value)
                    {
                        NaPregradiX = true;

                        if (AStarExample.koordYi[mygame.brojacKoord] == temp_y.value)
                        {
                            NaPregradiY = true;
                            break;
                        }
                    }
                    else 
                    {
                        NaPregradiX = false;
                        NaPregradiY = false;
                    }

                    temp_x = temp_x.next;
                    temp_y = temp_y.next;
                }

                // provjera gdje je zid
                bool zidDesno = false;
                bool zidLijevo = false;
                bool zidGore = false;
                bool zidDolje = false;

                Node temp_x1 = mygame.wall_area_x.first;
                Node temp_y1 = mygame.wall_area_y.first;

                // provjera jel zid desno od glave
                for(int i =  mygame.wall_area_x.length; i>0; i--)
                {
                    if (mygame.snake_area_x_2.last.value + 1 == temp_x1.value)
                    {
                        if (mygame.snake_area_y_2.last.value == temp_y1.value)
                        {
                            zidDesno = true;
                            break;
                        }
                    }
                    else 
                    {
                        zidDesno = false;
                    }

                    temp_x1 = temp_x1.next;
                    temp_y1 = temp_y1.next;
                }



                temp_x1 = mygame.wall_area_x.first;
                temp_y1 = mygame.wall_area_y.first;

                // provjera jel zid lijevo od glave
                for(int i =  mygame.wall_area_x.length; i>0; i--)
                {
                    if (mygame.snake_area_x_2.last.value - 1 == temp_x1.value)
                    {
                        if (mygame.snake_area_y_2.last.value == temp_y1.value)
                        {
                            zidLijevo = true;
                            break;
                        }
                    }
                    else 
                    {
                        zidLijevo = false;
                    }

                    temp_x1 = temp_x1.next;
                    temp_y1 = temp_y1.next;
                }


                temp_x1 = mygame.wall_area_x.first;
                temp_y1 = mygame.wall_area_y.first;

                // provjera jel zid gore od glave
                for(int i =  mygame.wall_area_x.length; i>0; i--)
                {
                    if (mygame.snake_area_x_2.last.value == temp_x1.value)
                    {
                        if (mygame.snake_area_y_2.last.value - 1 == temp_y1.value)
                        {
                            zidGore = true;
                            break;
                        }
                    }
                    else 
                    {
                        zidGore = false;
                    }

                    temp_x1 = temp_x1.next;
                    temp_y1 = temp_y1.next;
                }


                temp_x1 = mygame.wall_area_x.first;
                temp_y1 = mygame.wall_area_y.first;

                // provjera jel zid dolje od glave
                for(int i =  mygame.wall_area_x.length; i>0; i--)
                {
                    if (mygame.snake_area_x_2.last.value == temp_x1.value)
                    {
                        if (mygame.snake_area_y_2.last.value + 1 == temp_y1.value)
                        {
                            zidDolje = true;
                            break;
                        }
                    }
                    else 
                    {
                        zidDolje = false;
                    }

                    temp_x1 = temp_x1.next;
                    temp_y1 = temp_y1.next;
                }


                // provjera gdje je tijelo zmije
                bool zmijaDesno = false;
                bool zmijaLijevo = false;
                bool zmijaGore = false;
                bool zmijaDolje = false;

                Node temp_x1z = mygame.snake_area_x_2.first;
                Node temp_y1z = mygame.snake_area_y_2.first;

                // provjera jel tijelo zmije desno od glave
                for (int i = mygame.snake_area_x_2.length; i > 1; i--)
                {
                    if (mygame.snake_area_x_2.last.value + 1 == temp_x1z.value)
                    {
                        if (mygame.snake_area_y_2.last.value == temp_y1z.value)
                        {
                            zmijaDesno = true;
                            break;
                        }
                    }
                    else
                    {
                        zmijaDesno = false;
                    }

                    temp_x1z = temp_x1z.next;
                    temp_y1z = temp_y1z.next;
                }



                temp_x1z = mygame.snake_area_x_2.first;
                temp_y1z = mygame.snake_area_y_2.first;

                // provjera jel tijelo zmije lijevo od glave
                for (int i = mygame.snake_area_x_2.length; i > 1; i--)
                {
                    if (mygame.snake_area_x_2.last.value - 1 == temp_x1z.value)
                    {
                        if (mygame.snake_area_y_2.last.value == temp_y1z.value)
                        {
                            zmijaLijevo = true;
                            break;
                        }
                    }
                    else
                    {
                        zmijaLijevo = false;
                    }

                    temp_x1z = temp_x1z.next;
                    temp_y1z = temp_y1z.next;
                }


                temp_x1z = mygame.snake_area_x_2.first;
                temp_y1z = mygame.snake_area_y_2.first;

                // provjera jel tijelo zmije gore od glave
                for (int i = mygame.snake_area_x_2.length; i > 1; i--)
                {
                    if (mygame.snake_area_x_2.last.value == temp_x1z.value)
                    {
                        if (mygame.snake_area_y_2.last.value - 1 == temp_y1z.value)
                        {
                            zmijaGore = true;
                            break;
                        }
                    }
                    else
                    {
                        zmijaGore = false;
                    }

                    temp_x1z = temp_x1z.next;
                    temp_y1z = temp_y1z.next;
                }


                temp_x1z = mygame.snake_area_x_2.first;
                temp_y1z = mygame.snake_area_y_2.first;

                // provjera jel tijelo zmije dolje od glave
                for (int i = mygame.snake_area_x_2.length; i > 1; i--)
                {
                    if (mygame.snake_area_x_2.last.value == temp_x1z.value)
                    {
                        if (mygame.snake_area_y_2.last.value + 1 == temp_y1z.value)
                        {
                            zmijaDolje = true;
                            break;
                        }
                    }
                    else
                    {
                        zmijaDolje = false;
                    }

                    temp_x1z = temp_x1z.next;
                    temp_y1z = temp_y1z.next;
                }


                // ako nije jednak cvoru zida
                if(NaPregradiX == false && NaPregradiY == false)
                {
                    // AUTOMATSKO za kretanje zmije (brojacKord - 1 = 0 -> dakle indeks glave za x i za y)

                    // DESNO odreden iduci cvor za kretanje
                    if (AStarExample.koordXi[mygame.brojacKoord] == AStarExample.koordXi[mygame.brojacKoord - 1] + 1
                        && AStarExample.koordYi[mygame.brojacKoord] == AStarExample.koordYi[mygame.brojacKoord - 1])
                    {
                        if (mygame.snake_route_2 != 4 && zmijaDesno == false)
                        {
                            mygame.Move_Snake_Right_2();
                            mygame.snake_route_2 = 2;
                        }

                        else if (mygame.snake_route_2 != 4 && zmijaDesno == true)
                        {
                            if (zidGore == false && zmijaGore == false)
                            {
                                mygame.Move_Snake_Up_2();
                                mygame.snake_route_2 = 1;
                            }
                            else if (zidDolje == false && zmijaDolje == false)
                            {
                                mygame.Move_Snake_Down_2();
                                mygame.snake_route_2 = 3;
                            }
                        }

                        else if (mygame.snake_route_2 == 4 && zidGore == false && zmijaGore == false)
                        {
                            mygame.Move_Snake_Up_2();
                            mygame.snake_route_2 = 1;
                        }

                        else if (mygame.snake_route_2 == 4 && zidGore == true && zidDolje == false && zmijaDolje == false)
                        {
                            mygame.Move_Snake_Down_2();
                            mygame.snake_route_2 = 3;
                        }

                        else if (mygame.snake_route_2 == 4 && zidGore == true && zidDolje == false && zmijaDolje == true)
                        {
                            if (zidLijevo == false && zmijaLijevo == false)
                            {
                                mygame.Move_Snake_Left_2();
                                mygame.snake_route_2 = 4;
                            }
                        }
                    }

                    // LIJEVO odreden iduci cvor za kretanje
                    else if (AStarExample.koordXi[mygame.brojacKoord] == AStarExample.koordXi[mygame.brojacKoord - 1] - 1
                            && AStarExample.koordYi[mygame.brojacKoord] == AStarExample.koordYi[mygame.brojacKoord - 1])
                    {
                        if (mygame.snake_route_2 != 2 && zmijaLijevo == false)
                        {
                            mygame.Move_Snake_Left_2();
                            mygame.snake_route_2 = 4;
                        }

                        else if (mygame.snake_route_2 != 2 && zmijaLijevo == true)
                        {
                            
                            if (zidGore == false && zmijaGore == false)
                            {
                                mygame.Move_Snake_Up_2();
                                mygame.snake_route_2 = 1;
                            }
                            else if (zidDolje == false && zmijaDolje == false)
                            {
                                mygame.Move_Snake_Down_2();
                                mygame.snake_route_2 = 3;
                            }
                        }

                        else if (mygame.snake_route_2 == 2 && zidGore == false && zmijaGore == false)
                        {
                            mygame.Move_Snake_Up_2();
                            mygame.snake_route_2 = 1;
                        }

                        else if (mygame.snake_route_2 == 2 && zidGore == true && zidDolje == false && zmijaDolje == false)
                        {
                            mygame.Move_Snake_Down_2();
                            mygame.snake_route_2 = 3;
                        }

                        else if (mygame.snake_route_2 == 2 && zidGore == true && zidDolje == false && zmijaDolje == true)
                        {
                            if (zidDesno == false && zmijaDesno == false)
                            {
                                mygame.Move_Snake_Right_2();
                                mygame.snake_route_2 = 2;
                            }
                        }
                    }

                    // GORE odreden iduci cvor za kretanje
                    else if (AStarExample.koordXi[mygame.brojacKoord] == AStarExample.koordXi[mygame.brojacKoord - 1]
                            && AStarExample.koordYi[mygame.brojacKoord] == AStarExample.koordYi[mygame.brojacKoord - 1] - 1)
                    {
                        if (mygame.snake_route_2 != 3 && zmijaGore == false)
                        {
                            mygame.Move_Snake_Up_2();
                            mygame.snake_route_2 = 1;
                        }
                        else if (mygame.snake_route_2 != 3 && zmijaGore == true)
                        {
                            if(zidLijevo == false && zmijaLijevo == false)
                            {
                                mygame.Move_Snake_Left_2();
                                mygame.snake_route_2 = 4;
                            }
                            else if (zidDesno == false && zmijaDesno == false)
                            {
                                mygame.Move_Snake_Right_2();
                                mygame.snake_route_2 = 2;
                            }
                        }
                        else if (mygame.snake_route_2 == 3 && zidLijevo == false && zmijaLijevo == false)
                        {
                            mygame.Move_Snake_Left_2();
                            mygame.snake_route_2 = 4;
                        }
                        else if (mygame.snake_route_2 == 3 && zidLijevo == true && zidDesno == false && zmijaDesno == false)
                        {
                            mygame.Move_Snake_Right_2();
                            mygame.snake_route_2 = 2;
                        }
                        else if (mygame.snake_route_2 == 3 && zidLijevo == true && zidDesno == false && zmijaDesno == true)
                        {
                            if (zidDolje == false && zmijaDolje == false)
                            {
                                mygame.Move_Snake_Down_2();
                                mygame.snake_route_2 = 3;
                            }
                        }
                    }

                    // DOLJE odreden iduci cvor za kretanje
                    else if (AStarExample.koordXi[mygame.brojacKoord] == AStarExample.koordXi[mygame.brojacKoord - 1]
                            && AStarExample.koordYi[mygame.brojacKoord] == AStarExample.koordYi[mygame.brojacKoord - 1] + 1)
                    {
                        if (mygame.snake_route_2 != 1 && zmijaDolje == false)
                        {
                            mygame.Move_Snake_Down_2();
                            mygame.snake_route_2 = 3;
                        }

                        else if (mygame.snake_route_2 != 1 && zmijaDolje == true)
                        {
                            if (zidLijevo == false && zmijaLijevo == false)
                            {
                                mygame.Move_Snake_Left_2();
                                mygame.snake_route_2 = 4;
                            }
                            else if (zidDesno == false && zmijaDesno == false)
                            {
                                mygame.Move_Snake_Right_2();
                                mygame.snake_route_2 = 2;
                            }
                        }

                        else if (mygame.snake_route_2 == 1 && zidLijevo == false && zmijaLijevo == false)
                        {
                            mygame.Move_Snake_Left_2();
                            mygame.snake_route_2 = 4;
                        }

                        else if (mygame.snake_route_2 == 1 && zidLijevo == true && zidDesno == false && zmijaDesno == false)
                        {
                            mygame.Move_Snake_Right_2();
                            mygame.snake_route_2 = 2;
                        }

                        else if (mygame.snake_route_2 == 1 && zidLijevo == true && zidDesno == false && zmijaDesno == true)
                        {
                            if (zidGore == false && zmijaGore == false)
                            {
                                mygame.Move_Snake_Up_2();
                                mygame.snake_route_2 = 1;
                            }
                        }
                    }

                    // LIJEVO GORE odreden iduci cvor za kretanje
                    else if (AStarExample.koordXi[mygame.brojacKoord] == AStarExample.koordXi[mygame.brojacKoord - 1] - 1
                             && AStarExample.koordYi[mygame.brojacKoord] == AStarExample.koordYi[mygame.brojacKoord - 1] - 1)
                    {
                        if (mygame.snake_route_2 != 2)
                        {
                            mygame.Move_Snake_Left_2();
                            mygame.Move_Snake_Up_2();
                            mygame.snake_route_2 = 1;
                        }

                        else if (mygame.snake_route_2 == 2)
                        {
                            mygame.Move_Snake_Up_2();
                            mygame.Move_Snake_Left_2();
                            mygame.snake_route_2 = 4;
                        }
                    }

                    // LIJEVO DOLJE odreden iduci cvor za kretanje
                    else if (AStarExample.koordXi[mygame.brojacKoord] == AStarExample.koordXi[mygame.brojacKoord - 1] - 1
                             && AStarExample.koordYi[mygame.brojacKoord] == AStarExample.koordYi[mygame.brojacKoord - 1] + 1)
                    {
                        if (mygame.snake_route_2 != 2)
                        {
                            mygame.Move_Snake_Left_2();
                            mygame.Move_Snake_Down_2();
                            mygame.snake_route_2 = 3;
                        }

                        else if (mygame.snake_route_2 == 2)
                        {
                            mygame.Move_Snake_Down_2();
                            mygame.Move_Snake_Left_2();
                            mygame.snake_route_2 = 4;
                        }
                    }

                    // DESNO GORE odreden iduci cvor za kretanje
                    else if (AStarExample.koordXi[mygame.brojacKoord] == AStarExample.koordXi[mygame.brojacKoord - 1] + 1
                            && AStarExample.koordYi[mygame.brojacKoord] == AStarExample.koordYi[mygame.brojacKoord - 1] - 1)
                    {
                        if (mygame.snake_route_2 != 4)
                        {
                            mygame.Move_Snake_Right_2();
                            mygame.Move_Snake_Up_2();
                            mygame.snake_route_2 = 1;
                        }

                        else if (mygame.snake_route_2 == 4)
                        {
                            mygame.Move_Snake_Up_2();
                            mygame.Move_Snake_Right_2();
                            mygame.snake_route_2 = 2;
                        }
                    }

                    // DESNO DOLJE odreden iduci cvor za kretanje
                    else if (AStarExample.koordXi[mygame.brojacKoord] == AStarExample.koordXi[mygame.brojacKoord - 1] + 1
                            && AStarExample.koordYi[mygame.brojacKoord] == AStarExample.koordYi[mygame.brojacKoord - 1] + 1)
                    {
                        if (mygame.snake_route_2 != 4)
                        {
                            mygame.Move_Snake_Right_2();
                            mygame.Move_Snake_Down_2();
                            mygame.snake_route_2 = 3;
                        }

                        else if (mygame.snake_route_2 == 4)
                        {
                            mygame.Move_Snake_Down_2();
                            mygame.Move_Snake_Right_2();
                            mygame.snake_route_2 = 2;
                        }
                    }

                }
                else // if (AStarExample.pathfindSucceeded != true)
                {
                    /*
                    if (mygame.snake_route_2 == 1) mygame.Move_Snake_Up_2();
                    else if (mygame.snake_route_2 == 2) mygame.Move_Snake_Right_2();
                    else if (mygame.snake_route_2 == 3) mygame.Move_Snake_Down_2();
                    else if (mygame.snake_route_2 == 4) mygame.Move_Snake_Left_2();
                    */

                    zidDesno = false;
                    zidLijevo = false;
                    zidGore = false;
                    zidDolje = false;

                    temp_x1 = mygame.wall_area_x.first;
                    temp_y1 = mygame.wall_area_y.first;

                    // provjera jel zid desno od glave
                    for(int i =  mygame.wall_area_x.length; i>0; i--)
                    {
                        if (mygame.snake_area_x_2.last.value + 1 == temp_x1.value)
                        {
                            if (mygame.snake_area_y_2.last.value == temp_y1.value)
                            {
                                zidDesno = true;
                                break;
                            }
                        }
                        else 
                        {
                            zidDesno = false;
                        }

                        temp_x1 = temp_x1.next;
                        temp_y1 = temp_y1.next;
                    }



                    temp_x1 = mygame.wall_area_x.first;
                    temp_y1 = mygame.wall_area_y.first;

                    // provjera jel zid lijevo od glave
                    for(int i =  mygame.wall_area_x.length; i>0; i--)
                    {
                        if (mygame.snake_area_x_2.last.value - 1 == temp_x1.value)
                        {
                            if (mygame.snake_area_y_2.last.value == temp_y1.value)
                            {
                                zidLijevo = true;
                                break;
                            }
                        }
                        else 
                        {
                            zidLijevo = false;
                        }

                        temp_x1 = temp_x1.next;
                        temp_y1 = temp_y1.next;
                    }


                    temp_x1 = mygame.wall_area_x.first;
                    temp_y1 = mygame.wall_area_y.first;

                    // provjera jel zid gore od glave
                    for(int i =  mygame.wall_area_x.length; i>0; i--)
                    {
                        if (mygame.snake_area_x_2.last.value == temp_x1.value)
                        {
                            if (mygame.snake_area_y_2.last.value - 1== temp_y1.value)
                            {
                                zidGore = true;
                                break;
                            }
                        }
                        else 
                        {
                            zidGore = false;
                        }

                        temp_x1 = temp_x1.next;
                        temp_y1 = temp_y1.next;
                    }


                    temp_x1 = mygame.wall_area_x.first;
                    temp_y1 = mygame.wall_area_y.first;

                    // provjera jel zid dolje od glave
                    for(int i =  mygame.wall_area_x.length; i>0; i--)
                    {
                        if (mygame.snake_area_x_2.last.value == temp_x1.value)
                        {
                            if (mygame.snake_area_y_2.last.value + 1 == temp_y1.value)
                            {
                                zidDolje = true;
                                break;
                            }
                        }
                        else 
                        {
                            zidDolje = false;
                        }

                        temp_x1 = temp_x1.next;
                        temp_y1 = temp_y1.next;
                    }


                    if (mygame.snake_route_2 == 1)
                    {
                        if (zidGore == true)
                        { 
                            if(zidDesno == true)
                                mygame.Move_Snake_Left_2();
                            else if(zidLijevo == true)
                                mygame.Move_Snake_Right_2();
                        }
                        else
                            mygame.Move_Snake_Up_2();
                    }

                    if (mygame.snake_route_2 == 2)
                    {
                        if (zidDesno == true)
                        {
                            if (zidGore == true)
                                mygame.Move_Snake_Down_2();
                            else if (zidDolje == true)
                                mygame.Move_Snake_Up_2();
                        }
                        else
                            mygame.Move_Snake_Right_2();
                    }

                    if (mygame.snake_route_2 == 3)
                    {
                        if (zidDolje == true)
                        {
                            if (zidDesno == true)
                                mygame.Move_Snake_Left_2();
                            else if (zidLijevo == true)
                                mygame.Move_Snake_Right_2();
                        }
                        else
                            mygame.Move_Snake_Down_2();
                    }

                    if (mygame.snake_route_2 == 4)
                    {
                        if (zidLijevo == true)
                        {
                            if (zidGore == true)
                                mygame.Move_Snake_Down_2();
                            else if (zidDolje == true)
                                mygame.Move_Snake_Up_2();
                        }
                        else
                            mygame.Move_Snake_Left_2();
                    }
               }
            }
            catch 
            {
                bool zidDesno = false;
                bool zidLijevo = false;
                bool zidGore = false;
                bool zidDolje = false;

                Node temp_x1 = mygame.wall_area_x.first;
                Node temp_y1 = mygame.wall_area_y.first;

                // provjera jel zid desno od glave
                for(int i =  mygame.wall_area_x.length; i>0; i--)
                {
                    if (mygame.snake_area_x_2.last.value + 1 == temp_x1.value)
                    {
                        if (mygame.snake_area_y_2.last.value == temp_y1.value)
                        {
                            zidDesno = true;
                            break;
                        }
                    }
                    else 
                    {
                        zidDesno = false;
                    }

                    temp_x1 = temp_x1.next;
                    temp_y1 = temp_y1.next;
                }



                temp_x1 = mygame.wall_area_x.first;
                temp_y1 = mygame.wall_area_y.first;

                // provjera jel zid lijevo od glave
                for(int i =  mygame.wall_area_x.length; i>0; i--)
                {
                    if (mygame.snake_area_x_2.last.value - 1 == temp_x1.value)
                    {
                        if (mygame.snake_area_y_2.last.value == temp_y1.value)
                        {
                            zidLijevo = true;
                            break;
                        }
                    }
                    else 
                    {
                        zidLijevo = false;
                    }

                    temp_x1 = temp_x1.next;
                    temp_y1 = temp_y1.next;
                }


                temp_x1 = mygame.wall_area_x.first;
                temp_y1 = mygame.wall_area_y.first;

                // provjera jel zid gore od glave
                for(int i =  mygame.wall_area_x.length; i>0; i--)
                {
                    if (mygame.snake_area_x_2.last.value == temp_x1.value)
                    {
                        if (mygame.snake_area_y_2.last.value - 1== temp_y1.value)
                        {
                            zidGore = true;
                            break;
                        }
                    }
                    else 
                    {
                        zidGore = false;
                    }

                    temp_x1 = temp_x1.next;
                    temp_y1 = temp_y1.next;
                }


                temp_x1 = mygame.wall_area_x.first;
                temp_y1 = mygame.wall_area_y.first;

                // provjera jel zid dolje od glave
                for(int i =  mygame.wall_area_x.length; i>0; i--)
                {
                    if (mygame.snake_area_x_2.last.value == temp_x1.value)
                    {
                        if (mygame.snake_area_y_2.last.value + 1 == temp_y1.value)
                        {
                            zidDolje = true;
                            break;
                        }
                    }
                    else 
                    {
                        zidDolje = false;
                    }

                    temp_x1 = temp_x1.next;
                    temp_y1 = temp_y1.next;
                }


                if (mygame.snake_route_2 == 1)
                {
                    if (zidGore == true)
                    { 
                        if(zidDesno == true)
                            mygame.Move_Snake_Left_2();
                        else if(zidLijevo == true)
                            mygame.Move_Snake_Right_2();
                    }
                    else
                        mygame.Move_Snake_Up_2();
                }

                if (mygame.snake_route_2 == 2)
                {
                    if (zidDesno == true)
                    {
                        if (zidGore == true)
                            mygame.Move_Snake_Down_2();
                        else if (zidDolje == true)
                            mygame.Move_Snake_Up_2();
                    }
                    else
                        mygame.Move_Snake_Right_2();
                }

                if (mygame.snake_route_2 == 3)
                {
                    if (zidDolje == true)
                    {
                        if (zidDesno == true)
                            mygame.Move_Snake_Left_2();
                        else if (zidLijevo == true)
                            mygame.Move_Snake_Right_2();
                    }
                    else
                        mygame.Move_Snake_Down_2();
                }

                if (mygame.snake_route_2 == 4)
                {
                    if (zidLijevo == true)
                    {
                        if (zidGore == true)
                            mygame.Move_Snake_Down_2();
                        else if (zidDolje == true)
                            mygame.Move_Snake_Up_2();
                    }
                    else
                        mygame.Move_Snake_Left_2();
                }
            }




            // dodavanje novog cvora na zmiju
            mygame.Add_Snake_Node_2(mygame.node_x_2, mygame.node_y_2);
            mygame.snake_area_x_2.Add_Node(mygame.node_x_2);
            mygame.snake_area_y_2.Add_Node(mygame.node_y_2);
            mygame.snake_route_temp_2 = mygame.snake_route_2;


            if (!mygame.eated2)
            {
                if ((mygame.snake_area_x_2.first.value >= mygame.bait_rec.X)
                    && (mygame.snake_area_x_2.first.value <= (mygame.bait_rec.X + 4))
                    && (mygame.bait_rec.X != -1)
                    && (mygame.snake_area_y_2.first.value >= mygame.bait_rec.Y)
                    && (mygame.snake_area_y_2.first.value <= (mygame.bait_rec.Y + 4))
                    && (mygame.bait_rec.Y != -1))
                {

                    mygame.Delete_Snake_Node_2(mygame.snake_area_x_2.first.value, mygame.snake_area_y_2.first.value, 1);
                }
                else
                {
                    mygame.Delete_Snake_Node_2(mygame.snake_area_x_2.first.value, mygame.snake_area_y_2.first.value, 0);
                }

                mygame.snake_area_x_2.Delete_First_Node();
                mygame.snake_area_y_2.Delete_First_Node();
            }
            else mygame.eated2 = false;

            if (!mygame.bait_bool)
            {
                mygame.Find_Coordinate_For_Bait();
                if (!mygame.Control_Bait_Coordinate())
                {
                    mygame.Add_New_Bait(mygame.bait.X, mygame.bait.Y);
                }
                else
                {
                    mygame.Add_New_Bait(2, 2);
                }
            }



            // provjeravanje jeli zmija pojela hranu (običnu)
            if (mygame.Did_Snake_Eat_The_Bait_2())
            {
                mygame.Eated_Sound();
                mygame.Find_Coordinate_For_Bait();

                if (!mygame.Control_Bait_Coordinate())
                {
                    mygame.Add_New_Bait(mygame.bait.X, mygame.bait.Y);
                }
                else
                {
                    mygame.Add_New_Bait(2, 2);
                    mygame.bait_bool = false;
                }


                mygame.eated2 = true;

                if (MyGame_timer.Interval > 40)
                    MyGame_timer.Interval--;

            }


            pictureBox1.Image = mygame.bmp;
            pictureBox1.Refresh();



            if ((mygame.node_x_2 > (mygame.game_width / mygame.node_length - 1)) && mygame.snake_route_2 == 2)
            {
                mygame.node_x_2 = 0;
                mygame.Add_Snake_Node_2(mygame.node_x_2, mygame.node_y_2);
                mygame.snake_area_x_2.Add_Node(mygame.node_x_2);
                mygame.snake_area_y_2.Add_Node(mygame.node_y_2);
                if (!mygame.eated2)
                {
                    mygame.Delete_Snake_Node_2(mygame.snake_area_x_2.first.value, mygame.snake_area_y_2.first.value, 0);
                    mygame.snake_area_x_2.Delete_First_Node();
                    mygame.snake_area_y_2.Delete_First_Node();
                }
                else mygame.eated2 = false;

            }
            else if (mygame.node_x_2 < 0 && mygame.snake_route_2 == 4)
            {
                mygame.node_x_2 = (mygame.game_width / mygame.node_length - 1);
                mygame.Add_Snake_Node_2(mygame.node_x_2, mygame.node_y_2);
                mygame.snake_area_x_2.Add_Node(mygame.node_x_2);
                mygame.snake_area_y_2.Add_Node(mygame.node_y_2);
                if (!mygame.eated2)
                {
                    mygame.Delete_Snake_Node_2(mygame.snake_area_x_2.first.value, mygame.snake_area_y_2.first.value, 0);
                    mygame.snake_area_x_2.Delete_First_Node();
                    mygame.snake_area_y_2.Delete_First_Node();
                }
                else mygame.eated2 = false;
            }
            if ((mygame.node_y_2 > (mygame.game_height / mygame.node_length - 1)) && mygame.snake_route_2 == 3)
            {
                mygame.node_y_2 = 0;
                mygame.Add_Snake_Node_2(mygame.node_x_2, mygame.node_y_2);
                mygame.snake_area_x_2.Add_Node(mygame.node_x_2);
                mygame.snake_area_y_2.Add_Node(mygame.node_y_2);
                if (!mygame.eated2)
                {
                    mygame.Delete_Snake_Node_2(mygame.snake_area_x_2.first.value, mygame.snake_area_y_2.first.value, 0);
                    mygame.snake_area_x_2.Delete_First_Node();
                    mygame.snake_area_y_2.Delete_First_Node();
                }
                else mygame.eated2 = false;
            }
            else if (mygame.node_y_2 < 0 && mygame.snake_route_2 == 1)
            {
                mygame.node_y_2 = (mygame.game_height / mygame.node_length - 1);
                mygame.Add_Snake_Node_2(mygame.node_x_2, mygame.node_y_2);
                mygame.snake_area_x_2.Add_Node(mygame.node_x_2);
                mygame.snake_area_y_2.Add_Node(mygame.node_y_2);
                if (!mygame.eated2)
                {
                    mygame.Delete_Snake_Node_2(mygame.snake_area_x_2.first.value, mygame.snake_area_y_2.first.value, 0);
                    mygame.snake_area_x_2.Delete_First_Node();
                    mygame.snake_area_y_2.Delete_First_Node();
                }
                else mygame.eated2 = false;

            }
            return true;

        }
        public bool Game_Playing()
        {

            lb_level.Text = "LEVEL-" + mygame.game_level.ToString();
            lb_point.Text = mygame.point.ToString() + "/" + mygame.level_point.ToString();

            // ako smo na levelu 2 i duljina zmije postane jednaka 10 predi na novi level neovisno o bodovima
            if (mygame.game_level == 2 && mygame.snake_area_x.length >= 10)
            {
                // za ucitavanje novog levela
                mygame.point = mygame.level_point;
            }

            if (mygame.point >= mygame.level_point)
            {
                // za ucitavanje novog levela
                Clear_Level();
                return true;
            }

            if (mygame.Did_Snake_Crash_Byself(mygame.snake_route)) // provjera jeli se zmija sudarila sa zidom ili sama sa sobom
            {
                // ako je na podrucju na kojem je dozvoljeno da zmija prelazi preko sebe
                if ((mygame.snake_area_x.last.value >= mygame.bait_rec.X)
                    && (mygame.snake_area_x.last.value <= (mygame.bait_rec.X + 4))
                    && (mygame.bait_rec.X != -1)
                    && (mygame.snake_area_y.last.value >= mygame.bait_rec.Y)
                    && (mygame.snake_area_y.last.value <= (mygame.bait_rec.Y + 4))
                    && (mygame.bait_rec.Y != -1)
                    && mygame.bait_bool_rec != false)
                {
                    MyGame_timer.Enabled = true;
                    wait_timer.Enabled = false;

                    // za kretanje zmije
                    if (mygame.snake_route == 1) mygame.Move_Snake_Up();
                    else if (mygame.snake_route == 2) mygame.Move_Snake_Right();
                    else if (mygame.snake_route == 3) mygame.Move_Snake_Down();
                    else if (mygame.snake_route == 4) mygame.Move_Snake_Left();

                    return true;
                }
                //inace
                else
                {
                    MyGame_timer.Enabled = false;
                    wait_timer.Enabled = true;
                    mygame.wait_time = 20;
                    return true;
                }

            }

            // postavljanje smijera kretanja zmije
            if (mygame.snake_route == 1) mygame.Move_Snake_Up();
            else if (mygame.snake_route == 2) mygame.Move_Snake_Right();
            else if (mygame.snake_route == 3) mygame.Move_Snake_Down();
            else if (mygame.snake_route == 4) mygame.Move_Snake_Left();

            // dodavanje novog cvora na zmiju
            mygame.Add_Snake_Node(mygame.node_x, mygame.node_y);
            mygame.snake_area_x.Add_Node(mygame.node_x);
            mygame.snake_area_y.Add_Node(mygame.node_y);
            mygame.snake_route_temp = mygame.snake_route;


            if (!mygame.eated)
            {
                if ((mygame.snake_area_x.first.value >= mygame.bait_rec.X)
                    && (mygame.snake_area_x.first.value <= (mygame.bait_rec.X + 4))
                    && (mygame.bait_rec.X != -1)
                    && (mygame.snake_area_y.first.value >= mygame.bait_rec.Y)
                    && (mygame.snake_area_y.first.value <= (mygame.bait_rec.Y + 4))
                    && (mygame.bait_rec.Y != -1)
                    && mygame.bait_bool_rec != false)
                {

                    mygame.Delete_Snake_Node(mygame.snake_area_x.first.value, mygame.snake_area_y.first.value, 1);
                }
                else
                {
                    mygame.Delete_Snake_Node(mygame.snake_area_x.first.value, mygame.snake_area_y.first.value, 0);
                }

                mygame.snake_area_x.Delete_First_Node();
                mygame.snake_area_y.Delete_First_Node();
            }
            else mygame.eated = false;

            if (!mygame.bait_bool)
            {
                mygame.Find_Coordinate_For_Bait();
                if (!mygame.Control_Bait_Coordinate())
                {
                    mygame.Add_New_Bait(mygame.bait.X, mygame.bait.Y);
                }
                else
                {
                    mygame.Add_New_Bait(2, 2);
                }
            }

            if (!mygame.bait_bool_rec && mygame.point > 50)
            {
                mygame.Find_Coordinate_For_Bait_rec();
                //ode postavit while da dobre koordinate uvijek uzme
                if (!mygame.Control_Bait_Coordinate_rec())
                {
                    mygame.Add_New_Rectangle(mygame.bait_rec.X, mygame.bait_rec.Y);
                }
                else
                    mygame.bait_bool_rec = false;

            }

            // kontroliranje pojavljivanja specijalne hrane
            int vrijeme = DateTime.Now.Second;
            switch (vrijeme)
            {
                case 5:

                    if (!mygame.bait_bool_smjer)
                    {
                        mygame.Find_Coordinate_For_Bait_smjer();

                        if (!mygame.Control_Bait_Coordinate_smjer())
                        {

                            mygame.Add_New_Bait_smjer(mygame.bait_smjer.X, mygame.bait_smjer.Y);

                        }

                    }
                    break;

                case 6:

                    if (!mygame.bait_bool_dulj)
                    {
                        mygame.Find_Coordinate_For_Bait_dulj();

                        if (!mygame.Control_Bait_Coordinate_dulj())
                        {

                            mygame.Add_New_Bait_dulj(mygame.bait_dulj.X, mygame.bait_dulj.Y);

                        }

                    }
                    break;

              

                case 11:

                    if (!mygame.bait_bool_inc)
                    {
                        mygame.Find_Coordinate_For_Bait_inc();

                        if (!mygame.Control_Bait_Coordinate_inc())
                        {

                            mygame.Add_New_Bait_inc(mygame.bait_inc.X, mygame.bait_inc.Y);

                        }

                    }
                    break;

                case 23:
                    if (!mygame.bait_bool_dec)
                    {
                        mygame.Find_Coordinate_For_Bait_dec();

                        if (!mygame.Control_Bait_Coordinate_dec())
                        {

                            mygame.Add_New_Bait_dec(mygame.bait_dec.X, mygame.bait_dec.Y);

                        }
                    }

                    break;

                case 58:
                    if (!mygame.bait_bool_lv)
                    {
                        mygame.Find_Coordinate_For_Bait_lv();

                        if (!mygame.Control_Bait_Coordinate_lv())
                        {

                            mygame.Add_New_Bait_lv(mygame.bait_lv.X, mygame.bait_lv.Y);

                        }
                    }

                    break;

                default: break;
            }


            // provjeravanje jeli zmija pojela hranu (običnu)
            if (mygame.Did_Snake_Eat_The_Bait())
            {
                mygame.Eated_Sound();
                mygame.Find_Coordinate_For_Bait();

                if (!mygame.Control_Bait_Coordinate())
                {
                    mygame.Add_New_Bait(mygame.bait.X, mygame.bait.Y);
                }
                else
                {

                    mygame.Add_New_Bait(2, 2);
                    mygame.bait_bool = false;
                }


                mygame.eated = true;
                mygame.point += 10;
                if (MyGame_timer.Interval > 40)
                    MyGame_timer.Interval--;

            }


            // provjeravanje jeli zmija pojela hranu (za povecavanje bodova)
            if (mygame.Did_Snake_Eat_The_Bait_inc())
            {
                mygame.eated = true;
                mygame.point += 30;
                mygame.bait_bool_inc = false;

                mygame.incEated = true;
                mygame.incPostavljen = 0;

                // brisanje 
                mygame.bait_inc.X = -1;
                mygame.bait_inc.Y = -1;
            }

            // provjeravanje jeli zmija pojela hranu (za smanjivanje bodova)
            if (mygame.Did_Snake_Eat_The_Bait_dec())
            {
                mygame.bait_bool_dec = false;
                mygame.eated = true;

                mygame.decEated = true;
                mygame.decPostavljen = 0;

                if (mygame.point >= 20) mygame.point -= 20;
                else mygame.point = 0;

                // brisanje 
                mygame.bait_dec.X = -1;
                mygame.bait_dec.Y = -1;
            }

            // provjeravanje jeli zmija pojela hranu (za duljinu zmije)
            if (mygame.Did_Snake_Eat_The_Bait_dulj())
            {
                mygame.bait_bool_dulj = false;
                mygame.eated = true;

                mygame.duljEated = true;
                mygame.duljPostavljen = 0;

                Random rn = new Random();
                int curr_len = mygame.snake_length - 7;
                int len = rn.Next(-curr_len, 6); //max produljenje 5

                int temp = len;

                if (len >= 0)
                {
                    while (temp >= 0)
                    {
                        mygame.snake_area_x.Add_Node(mygame.node_x);
                        mygame.snake_area_y.Add_Node(mygame.node_y);
                        mygame.Add_Snake_Node(mygame.node_x, mygame.node_y);
                        mygame.snake_route_temp = mygame.snake_route;
                        temp--;
                    }
                }

                while (len < 0)
                {
                    mygame.Delete_Snake_Node(mygame.snake_area_x.first.value, mygame.snake_area_y.first.value, 0);

                    mygame.snake_area_x.Delete_First_Node();
                    mygame.snake_area_y.Delete_First_Node();

                    len++;
                }

                mygame.bait_dulj.X = -1;
                mygame.bait_dulj.Y = -1;
            }

            // provjeravanje jeli zmija pojela hranu (za promjenu smijera)
            if (mygame.Did_Snake_Eat_The_Bait_smjer())
            {   
                mygame.bait_bool_smjer = false;

                mygame.smjerEated = true;
                mygame.smjerPostavljen = 0;

                if (mygame.snake_route == 1) mygame.snake_route = 2;
                else if (mygame.snake_route == 2) mygame.snake_route = 3;
                else if (mygame.snake_route == 3) mygame.snake_route = 4;
                else if (mygame.snake_route == 4) mygame.snake_route = 1;

                mygame.bait_smjer.X = -1;
                mygame.bait_smjer.Y = -1;

                mygame.smjerZastava = 1;

            }

         


            // provjeravanje jeli zmija pojela hranu (za zavrsetak levela)
            if (mygame.Did_Snake_Eat_The_Bait_lv())
            {

                //mygame.eated = true;
                mygame.bait_bool_lv = false;

                mygame.lvEated = true;
                mygame.lvPostavljen = 0;

                // za prelazak na novi level
                mygame.point = mygame.level_point;

                mygame.bait_lv.X = -1;
                mygame.bait_lv.Y = -1;

                return true;
            }

            int vrijeme_novo = DateTime.Now.Second;
            switch (vrijeme_novo)
            {

                case 1:
                    if (!mygame.Did_Snake_Eat_The_Bait_lv() && !mygame.lvEated && mygame.lvPostavljen == 1)
                    {
                        mygame.Delete_bait_lv(mygame.bait_lv.X, mygame.bait_lv.Y);
                        mygame.bait_bool_lv = false;
                        mygame.bait_lv.X = -1;
                        mygame.bait_lv.Y = -1;

                        mygame.lvPostavljen = 0;
                    }
                    break;

                case 8:
                    if (!mygame.Did_Snake_Eat_The_Bait_smjer() && !mygame.smjerEated && mygame.smjerPostavljen == 1)
                    {
                        mygame.Delete_bait_smjer(mygame.bait_smjer.X, mygame.bait_smjer.Y);
                        mygame.bait_bool_smjer = false;
                        mygame.bait_smjer.X = -1;
                        mygame.bait_smjer.Y = -1;

                        mygame.smjerPostavljen = 0;
                    }
                    break;

                case 9:
                    if (!mygame.Did_Snake_Eat_The_Bait_dulj() && !mygame.duljEated && mygame.duljPostavljen == 1)
                    {
                        mygame.Delete_bait_dulj(mygame.bait_dulj.X, mygame.bait_dulj.Y);
                        mygame.bait_bool_dulj = false;
                        mygame.bait_dulj.X = -1;
                        mygame.bait_dulj.Y = -1;

                        mygame.duljPostavljen = 0;
                    }
                    break;

                case 14:
                    if (!mygame.Did_Snake_Eat_The_Bait_inc() && !mygame.incEated && mygame.incPostavljen == 1)
                    {
                        mygame.Delete_bait_inc(mygame.bait_inc.X, mygame.bait_inc.Y);
                        mygame.bait_bool_inc = false;
                        mygame.bait_inc.X = -1;
                        mygame.bait_inc.Y = -1;

                        mygame.incPostavljen = 0;
                    }
                    break;

                case 27:
                    if (!mygame.Did_Snake_Eat_The_Bait_dec() && !mygame.decEated && mygame.decPostavljen == 1)
                    {
                        mygame.Delete_bait_dec(mygame.bait_dec.X, mygame.bait_dec.Y);
                        mygame.bait_bool_dec = false;
                        mygame.bait_dec.X = -1;
                        mygame.bait_dec.Y = -1;

                        mygame.decPostavljen = 0;
                    }
                    break;


                default: break;

            }


            pictureBox1.Image = mygame.bmp;
            pictureBox1.Refresh();



            if ((mygame.node_x > (mygame.game_width / mygame.node_length - 1)) && mygame.snake_route == 2)
            {
                mygame.node_x = 0;
                mygame.Add_Snake_Node(mygame.node_x, mygame.node_y);
                mygame.snake_area_x.Add_Node(mygame.node_x);
                mygame.snake_area_y.Add_Node(mygame.node_y);
                if (!mygame.eated)
                {
                    mygame.Delete_Snake_Node(mygame.snake_area_x.first.value, mygame.snake_area_y.first.value, 0);
                    mygame.snake_area_x.Delete_First_Node();
                    mygame.snake_area_y.Delete_First_Node();
                }
                else mygame.eated = false;

            }
            else if (mygame.node_x < 0 && mygame.snake_route == 4)
            {
                mygame.node_x = (mygame.game_width / mygame.node_length - 1);
                mygame.Add_Snake_Node(mygame.node_x, mygame.node_y);
                mygame.snake_area_x.Add_Node(mygame.node_x);
                mygame.snake_area_y.Add_Node(mygame.node_y);
                if (!mygame.eated)
                {
                    mygame.Delete_Snake_Node(mygame.snake_area_x.first.value, mygame.snake_area_y.first.value, 0);
                    mygame.snake_area_x.Delete_First_Node();
                    mygame.snake_area_y.Delete_First_Node();
                }
                else mygame.eated = false;
            }
            if ((mygame.node_y > (mygame.game_height / mygame.node_length - 1)) && mygame.snake_route == 3)
            {
                mygame.node_y = 0;
                mygame.Add_Snake_Node(mygame.node_x, mygame.node_y);
                mygame.snake_area_x.Add_Node(mygame.node_x);
                mygame.snake_area_y.Add_Node(mygame.node_y);
                if (!mygame.eated)
                {
                    mygame.Delete_Snake_Node(mygame.snake_area_x.first.value, mygame.snake_area_y.first.value, 0);
                    mygame.snake_area_x.Delete_First_Node();
                    mygame.snake_area_y.Delete_First_Node();
                }
                else mygame.eated = false;
            }
            else if (mygame.node_y < 0 && mygame.snake_route == 1)
            {
                mygame.node_y = (mygame.game_height / mygame.node_length - 1);
                mygame.Add_Snake_Node(mygame.node_x, mygame.node_y);
                mygame.snake_area_x.Add_Node(mygame.node_x);
                mygame.snake_area_y.Add_Node(mygame.node_y);
                if (!mygame.eated)
                {
                    mygame.Delete_Snake_Node(mygame.snake_area_x.first.value, mygame.snake_area_y.first.value, 0);
                    mygame.snake_area_x.Delete_First_Node();
                    mygame.snake_area_y.Delete_First_Node();
                }
                else mygame.eated = false;

            }
            return true;

        }



        // kombinacije tipki u igri
        void MyGameKeyDown(object sender, KeyEventArgs e)
        {

            if (mygame.game_status == "menu_1" || mygame.game_status == "menu_2" || mygame.game_status == "menu_3" || mygame.game_status == "menu_4" ||
               mygame.game_status == "menu_5" || mygame.game_status == "about" || mygame.game_status == "instructions" || mygame.game_status == "settings_1"
               || mygame.game_status == "settings_2" || mygame.game_status == "settings_3" || mygame.game_status == "settings_4" || mygame.game_status == "settings_5"
               || mygame.game_status == "settings_6" || mygame.game_status == "settings_7" || mygame.game_status == "clear") // || mygame.game_status == "game_over" || mygame.game_status == "clear")
            {
                Menu_Control_For_Keyboard(e.KeyCode);
            }

            else if (mygame.game_status == "playing")
            {
                if ((e.KeyCode == Keys.Up) && (mygame.snake_route_temp != 3))
                {
                    mygame.snake_route = 1;

                }
                // ZA BRZO POMICANJE PREMA GORE
                else if ((e.KeyCode == Keys.Up) && (mygame.snake_route_temp != 3) && (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) ||
                            (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9))
                {
                    int value = 0;
                    switch (e.KeyCode)
                    {
                        case Keys.NumPad0:
                            value = 0;
                            break;
                        case Keys.NumPad1:
                            value = 1;
                            break;
                        case Keys.NumPad2:
                            value = 2;
                            break;
                        case Keys.NumPad3:
                            value = 3;
                            break;
                        case Keys.NumPad4:
                            value = 4;
                            break;
                        case Keys.NumPad5:
                            value = 5;
                            break;
                        case Keys.NumPad6:
                            value = 6;
                            break;
                        case Keys.NumPad7:
                            value = 7;
                            break;
                        case Keys.NumPad8:
                            value = 8;
                            break;
                        case Keys.NumPad9:
                            value = 9;
                            break;
                    }

                    for (int i = 0; i < value; ++i)
                    {
                        if (!mygame.Did_Snake_Crash_Byself(mygame.snake_route)
                            && mygame.point < mygame.level_point)
                        {
                            Game_Playing();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if ((e.KeyCode == Keys.Right) && (mygame.snake_route_temp != 4))
                {
                    mygame.snake_route = 2;
                }
                // ZA BRZO POMICANJE U DESNO
                else if ((e.KeyCode == Keys.Right) && (mygame.snake_route_temp != 4) && (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) ||
                            (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9))
                {
                    int value = 0;
                    switch (e.KeyCode)
                    {
                        case Keys.NumPad0:
                            value = 0;
                            break;
                        case Keys.NumPad1:
                            value = 1;
                            break;
                        case Keys.NumPad2:
                            value = 2;
                            break;
                        case Keys.NumPad3:
                            value = 3;
                            break;
                        case Keys.NumPad4:
                            value = 4;
                            break;
                        case Keys.NumPad5:
                            value = 5;
                            break;
                        case Keys.NumPad6:
                            value = 6;
                            break;
                        case Keys.NumPad7:
                            value = 7;
                            break;
                        case Keys.NumPad8:
                            value = 8;
                            break;
                        case Keys.NumPad9:
                            value = 9;
                            break;
                    }

                    for (int i = 0; i < value; ++i)
                    {
                        if (!mygame.Did_Snake_Crash_Byself(mygame.snake_route)
                            && mygame.point < mygame.level_point)
                        {
                            Game_Playing();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if ((e.KeyCode == Keys.Down) && (mygame.snake_route_temp != 1))
                {
                    mygame.snake_route = 3;
                }
                // ZA BRZO POMICANJE PREMA DOLJE
                else if ((e.KeyCode == Keys.Down) && (mygame.snake_route_temp != 1) && (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) ||
                            (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9))
                {
                    int value = 0;
                    switch (e.KeyCode)
                    {
                        case Keys.NumPad0:
                            value = 0;
                            break;
                        case Keys.NumPad1:
                            value = 1;
                            break;
                        case Keys.NumPad2:
                            value = 2;
                            break;
                        case Keys.NumPad3:
                            value = 3;
                            break;
                        case Keys.NumPad4:
                            value = 4;
                            break;
                        case Keys.NumPad5:
                            value = 5;
                            break;
                        case Keys.NumPad6:
                            value = 6;
                            break;
                        case Keys.NumPad7:
                            value = 7;
                            break;
                        case Keys.NumPad8:
                            value = 8;
                            break;
                        case Keys.NumPad9:
                            value = 9;
                            break;
                    }

                    for (int i = 0; i < value; ++i)
                    {
                        if (!mygame.Did_Snake_Crash_Byself(mygame.snake_route)
                            && mygame.point < mygame.level_point)
                        {
                            Game_Playing();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if ((e.KeyCode == Keys.Left) && (mygame.snake_route_temp != 2))
                {
                    mygame.snake_route = 4;
                }
                // ZA BRZO POMICANJE U LIJEVO
                else if ((e.KeyCode == Keys.Left) && (mygame.snake_route_temp != 2) && (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) ||
                            (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9))
                {
                    int value = 0;
                    switch (e.KeyCode)
                    {
                        case Keys.NumPad0:
                            value = 0;
                            break;
                        case Keys.NumPad1:
                            value = 1;
                            break;
                        case Keys.NumPad2:
                            value = 2;
                            break;
                        case Keys.NumPad3:
                            value = 3;
                            break;
                        case Keys.NumPad4:
                            value = 4;
                            break;
                        case Keys.NumPad5:
                            value = 5;
                            break;
                        case Keys.NumPad6:
                            value = 6;
                            break;
                        case Keys.NumPad7:
                            value = 7;
                            break;
                        case Keys.NumPad8:
                            value = 8;
                            break;
                        case Keys.NumPad9:
                            value = 9;
                            break;
                    }

                    for (int i = 0; i < value; ++i)
                    {
                        if (!mygame.Did_Snake_Crash_Byself(mygame.snake_route)
                            && mygame.point < mygame.level_point)
                        {
                            Game_Playing();
                        }
                        else
                        {
                            break;
                        }
                    }
                }





                // SHIFT + LIJEVO
                if ((e.KeyCode == Keys.Left) && (mygame.snake_route_temp != 2) && e.Shift)
                {
                    int value = 0;
                    value = mygame.game_width / mygame.node_length - 1;

                    int glava = mygame.snake_area_x.Get_The_Last_Node_Value();
                    //0 je min u lijevo
                    //while (glava > 1) // ovo je za bas do ruba
                    while (glava > 5 && mygame.smjerZastava != 1)
                    {
                        if (!mygame.Did_Snake_Crash_Byself(mygame.snake_route)
                            && mygame.point < mygame.level_point
                            && !mygame.Did_Snake_Eat_The_Bait_smjer()
                            && !(mygame.game_level == 2 && mygame.snake_area_x.length >= 10))
                        {
                            Game_Playing();
                            glava = mygame.snake_area_x.Get_The_Last_Node_Value();
                        }
                        else
                        {
                            break;
                        }
                    }
                    mygame.smjerZastava = 0;
                }
                // SHIFT + DESNO
                else if ((e.KeyCode == Keys.Right) && (mygame.snake_route_temp != 4) && e.Shift)
                {
                    int value = 0;
                    value = mygame.game_width / mygame.node_length - 1;

                    int glava = mygame.snake_area_x.Get_The_Last_Node_Value();
                    //value je maks u desno
                    //while (glava < value - 1) // ovo je do samog ruba
                    while (glava < value - 5 && mygame.smjerZastava != 1)
                    {
                        if (!mygame.Did_Snake_Crash_Byself(mygame.snake_route)
                            && mygame.point < mygame.level_point
                            && !mygame.Did_Snake_Eat_The_Bait_smjer()
                            && !(mygame.game_level == 2 && mygame.snake_area_x.length >= 10))
                        {
                            Game_Playing();
                            glava = mygame.snake_area_x.Get_The_Last_Node_Value();
                        }
                        else
                        {
                            break;
                        }
                    }
                    mygame.smjerZastava = 0;
                }
                // SHIFT + GORE
                else if ((e.KeyCode == Keys.Up) && (mygame.snake_route_temp != 3) && e.Shift)
                {
                    int value = 0;
                    value = mygame.game_height / mygame.node_length - 1;

                    int glava = mygame.snake_area_y.Get_The_Last_Node_Value();
                    // 0 je maksimum za gore
                    //while (glava > 1) // ovo je do samog ruba
                    while (glava > 5 && mygame.smjerZastava != 1)
                    {
                        if (!mygame.Did_Snake_Crash_Byself(mygame.snake_route)
                            && mygame.point < mygame.level_point
                            && !mygame.Did_Snake_Eat_The_Bait_smjer()
                            && !(mygame.game_level == 2 && mygame.snake_area_x.length >= 10))
                        {
                            Game_Playing();
                            glava = mygame.snake_area_y.Get_The_Last_Node_Value();
                        }
                        else
                        {
                            break;
                        }
                    }
                    mygame.smjerZastava = 0;
                }
                // SHIFT + DOLJE
                else if ((e.KeyCode == Keys.Down) && (mygame.snake_route_temp != 1) && e.Shift)
                {
                    int value = 0;
                    value = mygame.game_height / mygame.node_length - 1;

                    int glava = mygame.snake_area_y.Get_The_Last_Node_Value();

                    //while (glava < value - 1) // ovo je do samog ruba
                    while (glava < value - 5 && mygame.smjerZastava != 1)
                    {
                        if (!mygame.Did_Snake_Crash_Byself(mygame.snake_route)
                            && mygame.point < mygame.level_point
                            && !mygame.Did_Snake_Eat_The_Bait_smjer()
                            && !(mygame.game_level == 2 && mygame.snake_area_x.length >= 10))
                        {
                            Game_Playing();
                            glava = mygame.snake_area_y.Get_The_Last_Node_Value();
                        }
                        else
                        {
                            break;
                        }
                    }
                    mygame.smjerZastava = 0;
                }





                // ALT + W (prema GORE da se NE SUDARI sama sa sobom)
                if ((e.KeyCode == (Keys)Enum.Parse(typeof(Keys), mygame.tipkaZaGore, true))
                    && (mygame.snake_route_temp != 3)
                    && (mygame.snake_route_temp != 2)
                    && (mygame.snake_route_temp != 4)
                    && e.Alt)
                {
                    // trazimo minimum po y osi 
                    Node temp_y = mygame.snake_area_y.first;

                    //polje minimuma 
                    List<int> min_y = new List<int>();
                    min_y.Add(-1);

                    //privremeni minimum
                    int t_min_y = -1;

                    t_min_y = temp_y.value;

                    int brojac = 0; // sluzi da pogleda da u ravnini ima barem 2 cvora

                
                    // gledamo gdje su ravnine od interesa po x koordinati
                    List<int> drugaKordVrijednosti = new List<int>();
                    drugaKordVrijednosti.Add(-1);

                    Node temp_x = mygame.snake_area_x.first;

                    // duljine ravnina od interesa
                    List<int> duljine = new List<int>();
                    duljine.Add(-1);

                    int brojacDulj = 0;

                    for (int i = mygame.snake_area_y.length; i > 0; i--)
                    {
                        if (brojac >= 2 && (t_min_y > min_y[min_y.Count - 1] || t_min_y < min_y[min_y.Count - 1]))
                        {
                            if (min_y[min_y.Count - 1] != t_min_y)
                            {
                                min_y.Add(t_min_y);

                                drugaKordVrijednosti.Add(temp_x.previous.previous.value);
                            }
                        }

                        if (temp_y.value > t_min_y)
                        {
                            t_min_y = temp_y.value;

                            if (brojacDulj != 0 && brojac >= 2)
                                duljine.Add(brojacDulj);

                            brojac = 0;
                            brojacDulj = 0;
                        }
                        else if (temp_y.value < t_min_y)
                        {
                            t_min_y = temp_y.value;

                            if (brojacDulj != 0 && brojac >= 2)
                                duljine.Add(brojacDulj);

                            brojac = 0;
                            brojacDulj = 0;
                        }
                        else
                        {
                            brojac++;
                            brojacDulj++;
                        }



                        temp_y = temp_y.next;
                        temp_x = temp_x.next;
                    }

                    int glava_y = mygame.snake_area_y.Get_The_Last_Node_Value();
                    int glava_x = mygame.snake_area_x.Get_The_Last_Node_Value();


                    // trazenje vrijednosti do koje treba otici glava
                    temp_y = mygame.snake_area_y.first;
                    int min = mygame.game_height / mygame.node_length - 1;

                    for (int i = mygame.snake_area_y.length; i > 0; i--)
                    {
                        if (temp_y.value <= min)
                        {
                            min = temp_y.value;
                        }

                        temp_y = temp_y.next;
                    }

                    int j = 1;

                    
                    for (int i = min_y.Count; i > 0; i--)
                    {
                        if (j <= min_y.Count - 1)
                        {
                            if (min_y[j] <= glava_y)
                            {
                                if (glava_x <= drugaKordVrijednosti[j] && glava_x >= drugaKordVrijednosti[j] - duljine[j])
                                {
                                    min = min_y[j];
                                    break;
                                }

                                else if (glava_x >= drugaKordVrijednosti[j] && glava_x <= drugaKordVrijednosti[j] + duljine[j])
                                {
                                    min = min_y[j];
                                    break;
                                }
                            }

                            j++;
                        }
                        else
                        {
                            j++;
                        }
                    }


                    while (glava_y > min + 3 && glava_y > 1)
                    {
                        if (!mygame.Did_Snake_Crash_Byself(mygame.snake_route))
                        {
                            Game_Playing();
                            glava_y = mygame.snake_area_y.Get_The_Last_Node_Value();
                        }
                        else 
                        {
                            break;
                        }
                    }
                    
                }
                    
                // ALT + S (prema DOLJE da se NE SUDARI sama sa sobom)
                else if ((e.KeyCode == (Keys)Enum.Parse(typeof(Keys), mygame.tipkaZaDolje, true))
                    && (mygame.snake_route_temp != 1)
                    && (mygame.snake_route_temp != 2)
                    && (mygame.snake_route_temp != 4)
                    && e.Alt)
                {
                    // trazimo maksimum po y osi 
                    Node temp_y = mygame.snake_area_y.first;

                    //polje maksimum 
                    List<int> max_y = new List<int>();
                    max_y.Add(-1);

                    //privremeni maksimum
                    int t_max_y = -1;

                    t_max_y = temp_y.value;

                    int brojac = 0; // sluzi da pogleda da u ravnini ima barem 2 cvora


                    // gledamo gdje su ravnine od interesa po x koordinati
                    List<int> drugaKordVrijednosti = new List<int>();
                    drugaKordVrijednosti.Add(-1);

                    Node temp_x = mygame.snake_area_x.first;

                    // duljine ravnina od interesa
                    List<int> duljine = new List<int>();
                    duljine.Add(-1);

                    int brojacDulj = 0;

                    for (int i = mygame.snake_area_y.length; i > 0; i--)
                    {
                        if (brojac >= 2 && (t_max_y > max_y[max_y.Count - 1] || t_max_y < max_y[max_y.Count - 1]))
                        {
                            if (max_y[max_y.Count - 1] != t_max_y)
                            {
                                max_y.Add(t_max_y);

                                drugaKordVrijednosti.Add(temp_x.previous.previous.value);
                            }
                        }

                        if (temp_y.value > t_max_y)
                        {
                            t_max_y = temp_y.value;

                            if (brojacDulj != 0 && brojac >= 2)
                                duljine.Add(brojacDulj);

                            brojac = 0;
                            brojacDulj = 0;
                        }
                        else if (temp_y.value < t_max_y)
                        {
                            t_max_y = temp_y.value;

                            if (brojacDulj != 0 && brojac >= 2)
                                duljine.Add(brojacDulj);

                            brojac = 0;
                            brojacDulj = 0;
                        }
                        else
                        {
                            brojac++;
                            brojacDulj++;
                        }



                        temp_y = temp_y.next;
                        temp_x = temp_x.next;
                    }

                    int glava_y = mygame.snake_area_y.Get_The_Last_Node_Value();
                    int glava_x = mygame.snake_area_x.Get_The_Last_Node_Value();


                    // trazenje vrijednosti do koje treba otici glava
                    temp_y = mygame.snake_area_y.first;
                    int max = 0;

                    for (int i = mygame.snake_area_y.length; i > 0; i--)
                    {
                        if (temp_y.value >= max)
                        {
                            max = temp_y.value;
                        }

                        temp_y = temp_y.next;
                    }

                    int j = 1;

                    for (int i = max_y.Count; i > 0; i--)
                    {
                        if (j <= max_y.Count - 1)
                        {
                            if (max_y[j] >= glava_y)
                            {
                                if (glava_x <= drugaKordVrijednosti[j] && glava_x >= drugaKordVrijednosti[j] - duljine[j])
                                {
                                    max = max_y[j];
                                    break;
                                }

                                else if (glava_x >= drugaKordVrijednosti[j] && glava_x <= drugaKordVrijednosti[j] + duljine[j])
                                {
                                    max = max_y[j];
                                    break;
                                }
                            }

                            j++;
                        }
                        else
                        {
                            j++;
                        }
                    }

                    int visina = 0;
                    visina = mygame.game_height / mygame.node_length - 1;

                    while (glava_y < max - 3 && glava_y < visina - 1)
                    {
                        if (!mygame.Did_Snake_Crash_Byself(mygame.snake_route))
                        {
                            Game_Playing();
                            glava_y = mygame.snake_area_y.Get_The_Last_Node_Value();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                
                // ALT + A (prema LIJEVO da se NE SUDARI sama sa sobom)
                else if ((e.KeyCode == (Keys)Enum.Parse(typeof(Keys), mygame.tipkaZaLijevo, true))
                    && (mygame.snake_route_temp != 1)
                    && (mygame.snake_route_temp != 2)
                    && (mygame.snake_route_temp != 3)
                    && e.Alt)
                {
                    // trazimo minimum po y osi 
                    Node temp_x = mygame.snake_area_x.first;

                    //polje minimuma 
                    List<int> min_x = new List<int>();
                    min_x.Add(-1);

                    //privremeni minimum
                    int t_min_x = -1;

                    t_min_x = temp_x.value;

                    int brojac = 0; // sluzi da pogleda da u ravnini ima barem 2 cvora


                    // gledamo gdje su ravnine od interesa po y koordinati
                    List<int> drugaKordVrijednosti = new List<int>();
                    drugaKordVrijednosti.Add(-1);

                    Node temp_y = mygame.snake_area_y.first;

                    // duljine ravnina od interesa
                    List<int> duljine = new List<int>();
                    duljine.Add(-1);

                    int brojacDulj = 0;

                    for (int i = mygame.snake_area_x.length; i > 0; i--)
                    {
                        if (brojac >= 2 && (t_min_x > min_x[min_x.Count - 1] || t_min_x < min_x[min_x.Count - 1]))
                        {
                            if (min_x[min_x.Count - 1] != t_min_x)
                            {
                                min_x.Add(t_min_x);

                                drugaKordVrijednosti.Add(temp_y.previous.previous.value);
                            }
                        }

                        if (temp_x.value > t_min_x)
                        {
                            t_min_x = temp_x.value;

                            if (brojacDulj != 0 && brojac >= 2)
                                duljine.Add(brojacDulj);

                            brojac = 0;
                            brojacDulj = 0;
                        }
                        else if (temp_x.value < t_min_x)
                        {
                            t_min_x = temp_x.value;

                            if (brojacDulj != 0 && brojac >= 2)
                                duljine.Add(brojacDulj);

                            brojac = 0;
                            brojacDulj = 0;
                        }
                        else
                        {
                            brojac++;
                            brojacDulj++;
                        }



                        temp_y = temp_y.next;
                        temp_x = temp_x.next;
                    }

                    int glava_y = mygame.snake_area_y.Get_The_Last_Node_Value();
                    int glava_x = mygame.snake_area_x.Get_The_Last_Node_Value();


                    // trazenje vrijednosti do koje treba otici glava
                    temp_x = mygame.snake_area_x.first;
                    int min = mygame.game_width / mygame.node_length - 1;

                    for (int i = mygame.snake_area_x.length; i > 0; i--)
                    {
                        if (temp_x.value <= min)
                        {
                            min = temp_x.value;
                        }

                        temp_x = temp_x.next;
                    }

                    int j = 1;

                    for (int i = min_x.Count; i > 0; i--)
                    {
                        if (j <= min_x.Count - 1)
                        {
                            if (min_x[j] <= glava_x)
                            {
                                if (glava_y <= drugaKordVrijednosti[j] && glava_y >= drugaKordVrijednosti[j] - duljine[j])
                                {
                                    min = min_x[j];
                                    break;
                                }

                                else if (glava_y >= drugaKordVrijednosti[j] && glava_y <= drugaKordVrijednosti[j] + duljine[j])
                                {
                                    min = min_x[j];
                                    break;
                                }
                            }

                            j++;
                        }
                        else
                        {
                            j++;
                        }
                    }


                    while (glava_x > min + 3 && glava_x > 1)
                    {
                        if (!mygame.Did_Snake_Crash_Byself(mygame.snake_route))
                        {
                            Game_Playing();
                            glava_x = mygame.snake_area_x.Get_The_Last_Node_Value();
                        }
                        else
                        {
                            break;
                        }
                    }

                }

                // ALT + D (prema DESNO da se NE SUDARI sama sa sobom)
                else if ((e.KeyCode == (Keys)Enum.Parse(typeof(Keys), mygame.tipkaZaDesno, true))
                    && (mygame.snake_route_temp != 1)
                    && (mygame.snake_route_temp != 3)
                    && (mygame.snake_route_temp != 4)
                    && e.Alt)
                {
                    // trazimo maksimum po x osi 
                    Node temp_x = mygame.snake_area_x.first;

                    //polje maksimuma 
                    List<int> max_x = new List<int>();
                    max_x.Add(-1);

                    //privremeni maksimuma
                    int t_max_x = -1;

                    t_max_x = temp_x.value;

                    int brojac = 0; // sluzi da pogleda da u ravnini ima barem 2 cvora


                    // gledamo gdje su ravnine od interesa po y koordinati
                    List<int> drugaKordVrijednosti = new List<int>();
                    drugaKordVrijednosti.Add(-1);

                    Node temp_y = mygame.snake_area_y.first;

                    // duljine ravnina od interesa
                    List<int> duljine = new List<int>();
                    duljine.Add(-1);

                    int brojacDulj = 0;

                    for (int i = mygame.snake_area_x.length; i > 0; i--)
                    {
                        if (brojac >= 2 && (t_max_x > max_x[max_x.Count - 1] || t_max_x < max_x[max_x.Count - 1]))
                        {
                            if (max_x[max_x.Count - 1] != t_max_x)
                            {
                                max_x.Add(t_max_x);

                                drugaKordVrijednosti.Add(temp_y.previous.previous.value);
                            }
                        }

                        if (temp_x.value > t_max_x)
                        {
                            t_max_x = temp_x.value;

                            if (brojacDulj != 0 && brojac >= 2)
                                duljine.Add(brojacDulj);

                            brojac = 0;
                            brojacDulj = 0;
                        }
                        else if (temp_x.value < t_max_x)
                        {
                            t_max_x = temp_x.value;

                            if (brojacDulj != 0 && brojac >= 2)
                                duljine.Add(brojacDulj);

                            brojac = 0;
                            brojacDulj = 0;
                        }
                        else
                        {
                            brojac++;
                            brojacDulj++;
                        }



                        temp_y = temp_y.next;
                        temp_x = temp_x.next;
                    }

                    int glava_y = mygame.snake_area_y.Get_The_Last_Node_Value();
                    int glava_x = mygame.snake_area_x.Get_The_Last_Node_Value();


                    // trazenje vrijednosti do koje treba otici glava
                    temp_x = mygame.snake_area_x.first;
                    int max = 0;

                    for (int i = mygame.snake_area_x.length; i > 0; i--)
                    {
                        if (temp_x.value >= max)
                        {
                            max = temp_x.value;
                        }

                        temp_x = temp_x.next;
                    }

                    int j = 1;

                    for (int i = max_x.Count; i > 0; i--)
                    {
                        if (j <= max_x.Count - 1)
                        {
                            if (max_x[j] >= glava_x)
                            {
                                if (glava_y <= drugaKordVrijednosti[j] && glava_y >= drugaKordVrijednosti[j] - duljine[j])
                                {
                                    max = max_x[j];
                                    break;
                                }

                                else if (glava_y >= drugaKordVrijednosti[j] && glava_y <= drugaKordVrijednosti[j] + duljine[j])
                                {
                                    max = max_x[j];
                                    break;
                                }
                            }

                            j++;
                        }
                        else
                        {
                            j++;
                        }
                    }

                    int sirina = 0;
                    sirina = mygame.game_width / mygame.node_length - 1;

                    while (glava_x < max - 3 && glava_x < sirina - 1)
                    {
                        if (!mygame.Did_Snake_Crash_Byself(mygame.snake_route))
                        {
                            Game_Playing();
                            glava_x = mygame.snake_area_x.Get_The_Last_Node_Value();
                        }
                        else
                        {
                            break;
                        }
                    }

                }







                // Informativni prozor o specijalnoj hrani
                if (e.Control && e.KeyCode == Keys.I)
                {
                    MyGame_timer.Enabled = false;

                    infoForm f1 = new infoForm();

                    f1.ShowDialog();

                    if (e.KeyCode == Keys.Enter)
                    {
                        MyGame_timer.Enabled = true;
                    }

                }


                mygame.wait_time = 20;
                MyGame_timer.Enabled = true;
                wait_timer.Enabled = false;

                // PAUSE na stisak tipke "P"
                if (e.KeyCode == Keys.P) Pause();
            }

            else if (mygame.game_status == "paused")
            {
                if (e.KeyCode == Keys.P)
                    Pause();
            }

            // GAME OVER
            else if (mygame.game_status == "game_over" && e.KeyCode == Keys.Enter)
            {
                if (mygame.brojIskorZivota <= mygame.brojDozZivota)
                {
                    Level_Design();
                    Get_Ready();
                    Get_Ready_2();

                    // za stvaranje nove hrane
                    mygame.bait_bool = false;

                    if (!mygame.bait_bool)
                    {
                        mygame.Find_Coordinate_For_Bait();
                        if (!mygame.Control_Bait_Coordinate())
                        {

                            mygame.Add_New_Bait(mygame.bait.X, mygame.bait.Y);
                        }
                        else
                        {

                            mygame.Add_New_Bait(2, 2);
                        }
                    }


                    mygame.snake_route = 2;

                    // za ponistavanje starog podrucja na kojem zmija smije prelaziti preko sebe
                    mygame.bait_rec.X = -1;
                    mygame.bait_rec.Y = -1;
                    // za stvaranje novog podrucja na kojem zmija smije prelaziti preko sebe 
                    mygame.bait_bool_rec = false;

                    mygame.game_status = "playing";
                    MyGame_timer.Enabled = true;
                }

                else
                {
                    Game_Create_Again();

                    lb_point.Text = "";
                    lb_level.Text = "MENU";
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                }


            }
        }

        // kombinacije tipki u menu-u
        bool Menu_Control_For_Keyboard(Keys e)
        {
            //Menu-1 - ZAPOCNI IGRU 
            if (mygame.game_status == "menu_1")
            {

                if (e == Keys.Enter)
                {
                    mygame.sound_menu.Stop();
                    mygame.game_status = "playing";
                    mygame.snake_route = 2;
                    mygame.snake_route_2 = 2;
                    MyGame_timer.Enabled = true;
                    return true;
                }

                else if (e == Keys.Down)
                {

                    mygame.Main_Menu(2);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    mygame.game_status = "menu_2";
                    return true;
                }
            }

            //Menu-2 - NAREDBE
            else if (mygame.game_status == "menu_2")
            {
                if (e == Keys.Enter)
                {
                    mygame.Instructions();
                    mygame.game_status = "instructions";
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }
                else if (e == Keys.Down)
                {

                    mygame.Main_Menu(3);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    mygame.game_status = "menu_3";
                    return true;
                }
                else if (e == Keys.Up)
                {

                    mygame.Main_Menu(1);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    mygame.game_status = "menu_1";
                    return true;
                }
            }

            //Menu-3 - POSTAVKE
            else if (mygame.game_status == "menu_3")
            {
                if (e == Keys.Enter)
                {
                    mygame.Settings(1);
                    mygame.game_status = "settings_1";
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }
                else if (e == Keys.Down)
                {

                    mygame.Main_Menu(4);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    mygame.game_status = "menu_4";
                    return true;
                }
                else if (e == Keys.Up)
                {

                    mygame.Main_Menu(2);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    mygame.game_status = "menu_2";
                    return true;
                }
            }

            //Menu-4 - O NAMA
            else if (mygame.game_status == "menu_4")
            {
                if (e == Keys.Enter)
                {
                    mygame.About();
                    mygame.game_status = "about";
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Down)
                {

                    mygame.Main_Menu(5);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    mygame.game_status = "menu_5";
                    return true;
                }

                else if (e == Keys.Up)
                {

                    mygame.Main_Menu(3);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    mygame.game_status = "menu_3";
                    return true;
                }
            }

            //Menu-5 - IZLAZ
            else if (mygame.game_status == "menu_5")
            {
                if (e == Keys.Enter)
                {
                    Application.Exit();
                }

                else if (e == Keys.Up)
                {

                    mygame.Main_Menu(4);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    mygame.game_status = "menu_4";
                    return true;
                }
            }

            // kad smo u NAREDBE
            else if (mygame.game_status == "instructions")
            {
                if (e == Keys.Back)
                {
                    mygame.Main_Menu(2);
                    mygame.game_status = "menu_2";
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }
            }

            // kad smo u POSTAVKE - ZVUK
            else if (mygame.game_status == "settings_1")
            {
                if (e == Keys.Back)
                {
                    mygame.Main_Menu(3);
                    mygame.game_status = "menu_3";
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Down)
                {
                    mygame.game_status = "settings_2";
                    mygame.Settings(2);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Enter)
                {

                    if (mygame.sound_control == "on")
                    {
                        mygame.sound_control = "off";
                        mygame.sound_menu.Stop();


                    }
                    else
                    {
                        mygame.sound_control = "on";
                        mygame.sound_menu.PlayLooping();
                    }

                    mygame.Settings(1);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }
            }

            // kad smo u POSTAVKE - PRIKAZ
            else if (mygame.game_status == "settings_2")
            {
                if (e == Keys.Back)
                {
                    mygame.Main_Menu(3);
                    mygame.game_status = "menu_3";
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Up)
                {
                    mygame.game_status = "settings_1";
                    mygame.Settings(1);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Down)
                {
                    mygame.game_status = "settings_3";
                    mygame.Settings(3);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Enter)
                {
                    if (this.Size.Width < 791) this.Size = new Size(950, 740);
                    else if (this.Size.Width < 951) this.Size = new Size(1250, 740);
                    else if (this.Size.Width > 1249) this.Size = new Size(790, 740);

                    mygame.game_height = (this.Size.Height - 50) - ((this.Size.Height - 50) % mygame.node_length);
                    mygame.game_width = (this.Size.Width - 50) - ((this.Size.Width - 50) % mygame.node_length);

                    Game_Create_Again();
                    mygame.game_status = "settings_2";
                    mygame.Settings(2);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }
            }

            // kad smo u POSTAVKE - TEZINA
            else if (mygame.game_status == "settings_3")
            {
                if (e == Keys.Back)
                {
                    mygame.Main_Menu(3);
                    mygame.game_status = "menu_3";
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Up)
                {
                    mygame.game_status = "settings_2";
                    mygame.Settings(2);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Down)
                {
                    mygame.game_status = "settings_4";
                    mygame.Settings(4);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Enter)
                {
                    if (mygame.speed_control == "easy")
                    {
                        mygame.speed_control = "hard";
                        Set_Game_Speed(1);
                    }
                    else if (mygame.speed_control == "hard")
                    {
                        mygame.speed_control = "hardest";
                        Set_Game_Speed(2);
                    }
                    else if (mygame.speed_control == "hardest")
                    {
                        mygame.speed_control = "easy";
                        Set_Game_Speed(0);
                    }

                    mygame.game_status = "settings_3";
                    mygame.Settings(3);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }
            }

            // kad smo u POSTAVKE - KOMBINACIJE TIPKI

            // za namjestanje tipke za GORE da se ne sudare
            else if (mygame.game_status == "settings_4")
            {
                if (e == Keys.Back)
                {
                    mygame.Main_Menu(3);
                    mygame.game_status = "menu_3";
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Up)
                {
                    mygame.game_status = "settings_3";
                    mygame.Settings(3);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Down)
                {
                    mygame.game_status = "settings_5";
                    mygame.Settings(5);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e >= Keys.A && e <= Keys.Z)
                {
                    mygame.tipkaZaGore = e.ToString();

                    mygame.game_status = "settings_4";
                    mygame.Settings(4);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }
            }

            // za namjestanje tipke za DOLJE da se ne sudare
            else if (mygame.game_status == "settings_5")
            {
                if (e == Keys.Back)
                {
                    mygame.Main_Menu(3);
                    mygame.game_status = "menu_3";
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Up)
                {
                    mygame.game_status = "settings_4";
                    mygame.Settings(4);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Down)
                {
                    mygame.game_status = "settings_6";
                    mygame.Settings(6);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e >= Keys.A && e <= Keys.Z)
                {
                    mygame.tipkaZaDolje = e.ToString();

                    mygame.game_status = "settings_5";
                    mygame.Settings(5);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }
            }

            // za namjestanje tipke za LIJEVO da se ne sudare
            else if (mygame.game_status == "settings_6")
            {
                if (e == Keys.Back)
                {
                    mygame.Main_Menu(3);
                    mygame.game_status = "menu_3";
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Up)
                {
                    mygame.game_status = "settings_5";
                    mygame.Settings(5);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Down)
                {
                    mygame.game_status = "settings_7";
                    mygame.Settings(7);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e >= Keys.A && e <= Keys.Z)
                {
                    mygame.tipkaZaLijevo = e.ToString();

                    mygame.game_status = "settings_6";
                    mygame.Settings(6);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }
            }

            // za namjestanje tipke za DESNO da se ne sudare
            else if (mygame.game_status == "settings_7")
            {
                if (e == Keys.Back)
                {
                    mygame.Main_Menu(3);
                    mygame.game_status = "menu_3";
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e == Keys.Up)
                {
                    mygame.game_status = "settings_6";
                    mygame.Settings(6);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }

                else if (e >= Keys.A && e <= Keys.Z)
                {
                    mygame.tipkaZaDesno = e.ToString();

                    mygame.game_status = "settings_7";
                    mygame.Settings(7);
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }
            }

            // kad smo u O NAMA
            else if (mygame.game_status == "about")
            {
                if (e == Keys.Back)
                {
                    mygame.Main_Menu(4);
                    mygame.game_status = "menu_4";
                    pictureBox1.Image = mygame.menu_bmp;
                    pictureBox1.Refresh();
                    return true;
                }
            }

            // CLEAR LEVEL
            else if (mygame.game_status == "clear")
            {
                if (e == Keys.Enter)
                {
                    Level_Design();
                    Get_Ready();
                    Get_Ready_2();
                    mygame.game_status = "playing";
                    MyGame_timer.Enabled = true;
                }
            }


            Thread.Sleep(100);
            return true;
        }

        // Game over i idi u glavni menu
        void Game_Over()
        {
            mygame.Game_Over();
            pictureBox1.Refresh();
            mygame.game_status = "game_over";
        }


        // timer funckija
        void Wait_timerTick(object sender, EventArgs e)
        {
            mygame.wait_time--;
            if (mygame.wait_time == 0)
            {
                wait_timer.Enabled = false;
                Game_Over();
            }
        }

        // dizajn levela
        public void Level_Design()
        {

            int temp, temp2, y, i;

            mygame.Load_Graphic();
            mygame.node_x = 4;
            mygame.node_y = 4;

            //
            mygame.node_x_2 = 30;
            mygame.node_y_2 = 30;

            //
            mygame.snake_length_2 = 7;
            mygame.snake_length = 7;


            // postavljanje eksplicitno brzine koja je odabrana na pocetku
            // bitno jer na primjer ako bude game over na drugom nivou gjde je brzina
            // razlicita od postavljene u postavkama, ona bi ostala
            if (mygame.game_level == 1)
            {
                
                if (mygame.speed_control == "easy")
                {
                    Set_Game_Speed(0);
                    
                }
                else if (mygame.speed_control == "hard")
                {
                    Set_Game_Speed(1);
                }
                else if (mygame.speed_control == "hardest")
                {
                    Set_Game_Speed(2);
                }
            }

            //LEVEL-2 dizajn
            if (mygame.game_level == 2)
            {
                temp = mygame.game_width / mygame.node_length - 1;
                temp2 = mygame.game_height / mygame.node_length - 1;
                mygame.Add_Wall_Node(0, 0, temp, 0);
                mygame.Add_Wall_Node(temp, 0, temp, temp2);
                mygame.Add_Wall_Node(0, temp2, temp, temp2);
                mygame.Add_Wall_Node(0, 0, 0, temp2);


                // povecanje brzine u drugom levelu (i tako u svakom narednom)
                if (mygame.speed_control == "easy")
                {
                    MyGame_timer.Interval = 150-20;
                }
                else if (mygame.speed_control == "hard")
                {
                    MyGame_timer.Interval = 100 - 20;
                }
                else if (mygame.speed_control == "hardest")
                {
                    MyGame_timer.Interval = 75 - 20;
                }

            }
            //LEVEL-3 dizajn
            else if (mygame.game_level == 3)
            {
                int temp_x = (mygame.game_width / mygame.node_length) / 4;
                int temp_y = (mygame.game_height / mygame.node_length) / 4;

                mygame.Add_Wall_Node(temp_x, temp_y, temp_x + temp_x / 2, temp_y);
                mygame.Add_Wall_Node(temp_x, temp_y, temp_x, temp_y + temp_y / 2);

                mygame.Add_Wall_Node(temp_x, 2 * temp_y + temp_y / 2 + 1, temp_x, 3 * temp_y);
                mygame.Add_Wall_Node(temp_x, 3 * temp_y, temp_x + temp_x / 2, 3 * temp_y);

                mygame.Add_Wall_Node(2 * temp_x + temp_x / 2 + 1, 3 * temp_y, 3 * temp_x, 3 * temp_y);
                mygame.Add_Wall_Node(3 * temp_x, 3 * temp_y - temp_y / 2, 3 * temp_x, 3 * temp_y);

                mygame.Add_Wall_Node(2 * temp_x + temp_x / 2 + 1, temp_y, 3 * temp_x, temp_y);
                mygame.Add_Wall_Node(3 * temp_x, temp_y, 3 * temp_x, temp_y + temp_y / 2);

                // povecanje brzine
                if (mygame.speed_control == "easy")
                {
                    MyGame_timer.Interval = 150 - 25;
                }
                else if (mygame.speed_control == "hard")
                {
                    MyGame_timer.Interval = 100 - 25;
                }
                else if (mygame.speed_control == "hardest")
                {
                    MyGame_timer.Interval = 75 - 25;
                }

            }
            //LEVEL-4 dizajn
            else if (mygame.game_level == 4)
            {

                temp = mygame.game_width / mygame.node_length - 1;
                y = 2;
                i = 5;

                while (i > 0)
                {
                    mygame.Add_Wall_Node(3, y, temp - 3, y);
                    y = y + 10;
                    i--;
                }

                // povecanje brzine
                if (mygame.speed_control == "easy")
                {
                    MyGame_timer.Interval = 150 - 30;
                }
                else if (mygame.speed_control == "hard")
                {
                    MyGame_timer.Interval = 100 - 30;
                }
                else if (mygame.speed_control == "hardest")
                {
                    MyGame_timer.Interval = 75 - 30;
                }
            }
            //LEVEL-5 dizajn
            else if (mygame.game_level == 5)
            {
                temp = mygame.game_width / mygame.node_length - 1;
                y = 6;
                i = 3;

                while (i > 0)
                {
                    mygame.Add_Wall_Node(6, y, temp - 6, y);
                    y = y + 10;
                    i--;
                }
                y = 6;

                mygame.Add_Wall_Node(6, y, 6, y + 10);
                mygame.Add_Wall_Node(temp - 6, y + 10, temp - 6, y + 20);


                // povecanje brzine
                if (mygame.speed_control == "easy")
                {
                    MyGame_timer.Interval = 150 - 35;

                }
                else if (mygame.speed_control == "hard")
                {
                    MyGame_timer.Interval = 100 - 35;
                }
                else if (mygame.speed_control == "hardest")
                {
                    MyGame_timer.Interval = 75 - 35;
                }


            }
            //LEVEL-6 dizajn
            else if (mygame.game_level == 6)
            {
                // eksplicitno postavljanje velicine zaslona na ovu jer je ona prilagođena Astar mapi za taj level
                // inace bi morali napraviti 3 mape, jednu za svaku dimenziju prozora i onda u zavisnosti od odabrane 
                // koristiti pripadnu mapu
                this.Size = new Size(1250, 740);

                mygame.game_height = (this.Size.Height - 50) - ((this.Size.Height - 50) % mygame.node_length);
                mygame.game_width = (this.Size.Width - 50) - ((this.Size.Width - 50) % mygame.node_length);

                int h = mygame.game_height;
                int w = mygame.game_width;
                //mygame = new Game(h, w);
                top_menu_panel.Size = new Size(w, top_menu_panel.Size.Height);

                mygame.bmp = new Bitmap(mygame.game_width, mygame.game_height);
                mygame.graph = Graphics.FromImage(mygame.bmp);
                mygame.graph.FillRectangle(mygame.back_color, 0, 0, mygame.game_width, mygame.game_height);
                //

                temp = mygame.game_width / mygame.node_length - 1;
                y = 6;
                i = 3;

                while (i > 0)
                {
                    mygame.Add_Wall_Node(6, y, temp - 6, y);
                    y = y + 10;
                    i--;
                }
                y = 6;

                mygame.Add_Wall_Node(6, y, 6, y + 10);
                mygame.Add_Wall_Node(temp - 6, y + 10, temp - 6, y + 20);


                temp = mygame.game_width / mygame.node_length - 1;
                temp2 = mygame.game_height / mygame.node_length - 1;
                mygame.Add_Wall_Node(0, 0, temp, 0);
                mygame.Add_Wall_Node(temp, 0, temp, temp2);
                mygame.Add_Wall_Node(0, temp2, temp, temp2);
                mygame.Add_Wall_Node(0, 0, 0, temp2);

                // povecanje brzine 
                if (mygame.speed_control == "easy")
                {
                    MyGame_timer.Interval = 150 - 45;
                }
                else if (mygame.speed_control == "hard")
                {
                    MyGame_timer.Interval = 100 - 45;
                }
                else if (mygame.speed_control == "hardest")
                {
                    MyGame_timer.Interval = 75 - 45;
                }


            }

        }

        // PAUZA funkcija
        public void Pause()
        {
            if (MyGame_timer.Enabled && mygame.brojIskorPauza <= mygame.brojDozPauza)
            {
                mygame.game_status = "paused";
                MyGame_timer.Enabled = false;
                mygame.Pause();
                pictureBox1.Image = mygame.menu_bmp;
                pictureBox1.Refresh();
            }

            else
            {
                mygame.game_status = "playing";
                MyGame_timer.Enabled = true;
            }
        }

        // CLEAR LEVEL funkcija
        public void Clear_Level()
        {
            //MessageBox.Show("Clear level");
            int level = mygame.game_level + 1;
            int game_height = mygame.game_height;
            int game_width = mygame.game_width;
            string sound = mygame.sound_control;

            mygame.bait_bool_lv = false;

            MyGame_timer.Enabled = false;

            if (level > mygame.max_level)
            {
                level = 1;
                mygame.Finish_Game();
                pictureBox1.Refresh();
                mygame.game_status = "game_over";

                // ako predemo igru zamrzni poruku cestitke za 3 sekunde
                Thread.Sleep(3000);

                // i opet pokreni igru od pocetka
                Game_Create_Again();
            }
            else
            {
                mygame.Clear_Level();

                pictureBox1.Refresh();
                string brzina = mygame.speed_control;
                mygame = new Game(game_height, game_width);
                mygame.speed_control = brzina;
                mygame.game_status = "clear";
                mygame.sound_control = sound;
                mygame.game_level = level;
            }
        }

        //BRZINA IGRE funkcija
        public void Set_Game_Speed(int i)
        {
            MyGame_timer.Interval = mygame.snake_speed[i];
        }


        // ponovno kreiranje igre zbog promjene velicine
        void Game_Create_Again()
        {
            string sound = mygame.sound_control;
            int h = mygame.game_height;
            int w = mygame.game_width;
            mygame = new Game(h, w);
            top_menu_panel.Size = new Size(w, top_menu_panel.Size.Height);

            mygame.sound_control = sound;
            MyGame_Form_Load(this, System.EventArgs.Empty);

            mygame.bait_bool = false;
            if (!mygame.bait_bool)
            {
                mygame.Find_Coordinate_For_Bait();
                if (!mygame.Control_Bait_Coordinate())
                {

                    mygame.Add_New_Bait(mygame.bait.X, mygame.bait.Y);
                }
                else
                {

                    mygame.Add_New_Bait(2, 2);
                }
            }
        }




    }
}