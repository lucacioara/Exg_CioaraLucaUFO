using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UFOLandingOnMars
{
    public class MarsForm : Form
    {
        public MarsForm()
        {
            this.Text = "UFO Landing on Mars";
            this.ClientSize = new Size(800, 600);
            this.Paint += new PaintEventHandler(DrawScene);
        }

        private void DrawScene(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.Clear(Color.Black);

            Rectangle groundRect = new Rectangle(0, 400, 800, 200);
            using (LinearGradientBrush groundBrush = new LinearGradientBrush(groundRect, Color.DarkRed, Color.OrangeRed, LinearGradientMode.Vertical))
            {
                g.FillRectangle(groundBrush, groundRect);
            }

            DrawFractalMountains(g, new Point(0, 400), new Point(800, 400), 80, 6);

            Point[][] rocks =
            {
                new Point[] { new Point(100, 500), new Point(150, 450), new Point(200, 500) },
                new Point[] { new Point(300, 520), new Point(350, 470), new Point(400, 520) },
                new Point[] { new Point(600, 480), new Point(650, 440), new Point(700, 480) }
            };

            foreach (var rock in rocks)
            {
                using (PathGradientBrush rockBrush = new PathGradientBrush(rock))
                {
                    rockBrush.CenterColor = Color.Gray;
                    rockBrush.SurroundColors = new[] { Color.DarkGray };
                    g.FillPolygon(rockBrush, rock);
                }

                using (Pen rockPen = new Pen(Color.Black, 2))
                {
                    g.DrawPolygon(rockPen, rock);
                }
            }

            DrawUFO(g);

            DrawEllipticalCraters(g);
        }

        private void DrawUFO(Graphics g)
        {
            Point[] ufoBase = { new Point(300, 300), new Point(500, 300), new Point(450, 350), new Point(350, 350) };
            using (LinearGradientBrush ufoBaseBrush = new LinearGradientBrush(new Rectangle(300, 300, 200, 50), Color.Silver, Color.Gray, LinearGradientMode.Vertical))
            {
                g.FillPolygon(ufoBaseBrush, ufoBase);
            }
            using (Pen ufoBasePen = new Pen(Color.Black, 2))
            {
                g.DrawPolygon(ufoBasePen, ufoBase);
            }

            using (GraphicsPath domePath = new GraphicsPath())
            {
                domePath.AddBezier(320, 300, 380, 250, 420, 250, 480, 300);
                domePath.AddLine(480, 300, 320, 300);
                using (PathGradientBrush domeBrush = new PathGradientBrush(domePath))
                {
                    domeBrush.CenterColor = Color.LightBlue;
                    domeBrush.SurroundColors = new[] { Color.Blue };
                    g.FillPath(domeBrush, domePath);
                }
                using (Pen domePen = new Pen(Color.Black, 2))
                {
                    g.DrawPath(domePen, domePath);
                }
            }

            Point[][] ufoLights =
            {
                new Point[] { new Point(310, 350), new Point(320, 360), new Point(330, 350) },
                new Point[] { new Point(370, 350), new Point(380, 360), new Point(390, 350) },
                new Point[] { new Point(430, 350), new Point(440, 360), new Point(450, 350) }
            };

            foreach (var light in ufoLights)
            {
                using (LinearGradientBrush lightBrush = new LinearGradientBrush(light[0], light[2], Color.Yellow, Color.Orange))
                {
                    g.FillPolygon(lightBrush, light);
                }
                using (Pen lightPen = new Pen(Color.Black, 1))
                {
                    g.DrawPolygon(lightPen, light);
                }
            }

            // Interconnecting Bezier Curves
            using (Pen connectorPen = new Pen(Color.DarkGray, 2))
            {
                g.DrawBezier(connectorPen, new Point(310, 350), new Point(340, 370), new Point(360, 370), new Point(390, 350));
                g.DrawBezier(connectorPen, new Point(370, 350), new Point(400, 370), new Point(420, 370), new Point(450, 350));
            }
        }

        private void DrawFractalMountains(Graphics g, Point start, Point end, int heightVariation, int depth)
        {
            if (depth == 0)
            {
                using (Pen mountainPen = new Pen(Color.DarkSlateGray, 2))
                {
                    g.DrawLine(mountainPen, start, end);
                }
                return;
            }

            Random rand = new Random();
            int midX = (start.X + end.X) / 2;
            int midY = (start.Y + end.Y) / 2 - rand.Next(-heightVariation, heightVariation);
            Point mid = new Point(midX, midY);

            DrawFractalMountains(g, start, mid, heightVariation / 2, depth - 1);
            DrawFractalMountains(g, mid, end, heightVariation / 2, depth - 1);

            using (SolidBrush mountainBrush = new SolidBrush(Color.DarkSlateGray))
            {
                Point[] mountainPoints = { start, mid, end };
                g.FillPolygon(mountainBrush, mountainPoints);
            }
        }

        private void DrawEllipticalCraters(Graphics g)
        {
            Rectangle crater1 = new Rectangle(200, 450, 150, 50);
            Rectangle crater2 = new Rectangle(500, 480, 180, 60);

            using (LinearGradientBrush craterBrush = new LinearGradientBrush(new Point(0, 450), new Point(0, 500), Color.DarkOrange, Color.Red))
            {
                g.FillEllipse(craterBrush, crater1);
                g.FillEllipse(craterBrush, crater2);
            }

            using (Pen craterPen = new Pen(Color.Black, 2))
            {
                g.DrawEllipse(craterPen, crater1);
                g.DrawEllipse(craterPen, crater2);
            }

            DrawVoronoiOrganisms(g, crater2);
            DrawMulticellularOrganisms(g, crater1);
        }
        private void DrawVoronoiOrganisms(Graphics g, Rectangle crater)
        {
            Random rand = new Random();
            int organismCount = 20;
            PointF[] points = new PointF[organismCount];

            for (int i = 0; i < organismCount; i++)
            {
                float x = rand.Next(crater.Left, crater.Right);
                float y = rand.Next(crater.Top, crater.Bottom);
                points[i] = new PointF(x, y);
            }

            foreach (var point in points)
            {
                float radius = rand.Next(5, 15);
                RectangleF cell = new RectangleF(point.X - radius / 2, point.Y - radius / 2, radius, radius);

                using (LinearGradientBrush cellBrush = new LinearGradientBrush(cell, Color.LightGreen, Color.DarkGreen, LinearGradientMode.ForwardDiagonal))
                {
                    g.FillEllipse(cellBrush, cell);
                }

                using (Pen cellPen = new Pen(Color.Black, 1))
                {
                    g.DrawEllipse(cellPen, cell);
                }
            }
        }
        private void DrawMulticellularOrganisms(Graphics g, Rectangle crater)
        {
            Random rand = new Random();

            Point[] positions =
            {
              new Point(crater.Left - 20, crater.Top),
              new Point(crater.Right + 20, crater.Top),
              new Point(crater.Left + crater.Width / 2, crater.Bottom + 20),
              new Point(crater.Left - 30, crater.Bottom + 10),
              new Point(crater.Right + 30, crater.Bottom + 10)
    };

            foreach (Point position in positions)
            {
                DrawLSystemOrganism(g, position, -Math.PI / 2, 60, 3);
            }
        }
        private void DrawLSystemOrganism(Graphics g, Point start, double angle, int length, int depth)
        {
            if (depth == 0 || length <= 2)
                return;

            Point end = new Point(
                start.X + (int)(Math.Cos(angle) * length),
                start.Y + (int)(Math.Sin(angle) * length)
            );

            using (Pen branchPen = new Pen(Color.FromArgb(100 + depth * 50, Color.Green), depth))
            {
                g.DrawLine(branchPen, start, end);
            }

            double angleVariation = Math.PI / 6; 
            int newLength = length - 10; 

            DrawLSystemOrganism(g, end, angle - angleVariation, newLength, depth - 1);
            DrawLSystemOrganism(g, end, angle + angleVariation, newLength, depth - 1);
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MarsForm());
        }
    }
}
