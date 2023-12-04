using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS2023_Prototype
{
   internal class Utilitaires
   {
      public const int LARGEUR_CARTE_PIXEL = 1000;
      public const int HAUTEUR_CARTE_PIXEL = 1000;
      public const int TAILLE_CASE = 50;
      public const int NB_CASES_LARGE =
         LARGEUR_CARTE_PIXEL / TAILLE_CASE;
      public const int NB_CASES_HAUT =
         HAUTEUR_CARTE_PIXEL / TAILLE_CASE;
      public const int NB_BÂTIMENTS = 14;

      public class Bombe
      {
         public Point Position { get; set; }
         public Sprite Image { get; set; }
         public Bombe(Point position, Sprite image)
         {
            Position = position;
            Image = image;
         }
      }
      public static Bombe[] CréerBombes(int nbBombes, Point pos, Sprite img)
      {
         Bombe [] bombes = new Bombe[nbBombes];
         for(int i = 0; i != bombes.Length; ++i)
         {
            bombes[i] = new(pos, img);
         }
         return bombes;
      }
      public static Point GénérerPosition(Random générateur)
      {
         return new(générateur.Next(1, NB_CASES_LARGE - 1),
                    générateur.Next(1, NB_CASES_HAUT - 1));
      }
      public static Point GénérerPosition(Random générateur, Objet[,] carte, Objet requis)
      {
         Point pos = GénérerPosition(générateur);
         while (carte[pos.X, pos.Y] != requis)
         {
            pos =  GénérerPosition(générateur);
         }
         return pos;
      }
      public enum Objet { Vide, Force, Bâtiment }
      public enum DéplacementSpiderMan
      {
         GrimpeDroit, GrimpeHaut, GrimpeGauche,
         ToileGauche, ToileDroite, ToileHaut,
         Chute
      }
      public enum DéplacementGobelinVert { Droite, Haut, Gauche, Bas }
      public static void LancerBombe(Point p, Bombe[] bombes)
      {
         int i = 0;
         while (i != bombes.Length &&
                PartieÉtudiante.EstPositionValide(bombes[i].Position))
         {
            ++i;
         }
         if(i != bombes.Length)
         {
            bombes[i].Position = p;
            bombes[i].Image.Position =
               new(p.X * TAILLE_CASE, p.Y * TAILLE_CASE);
         }
      }
      public static DéplacementSpiderMan ConvertirToucheEnDéplacement(ConsoleKey ck)
      {
         DéplacementSpiderMan déplacement;
         switch (ck)
         {
            case ConsoleKey.A:
               déplacement = DéplacementSpiderMan.GrimpeGauche;
               break;
            case ConsoleKey.W:
               déplacement = DéplacementSpiderMan.GrimpeHaut;
               break;
            case ConsoleKey.D:
               déplacement = DéplacementSpiderMan.GrimpeDroit;
               break;
            case ConsoleKey.RightArrow:
               déplacement = DéplacementSpiderMan.ToileDroite;
               break;
            case ConsoleKey.LeftArrow:
               déplacement = DéplacementSpiderMan.ToileGauche;
               break;
            case ConsoleKey.UpArrow:
               déplacement = DéplacementSpiderMan.ToileHaut;
               break;
            default:
               déplacement = DéplacementSpiderMan.Chute;
               break;
         }
         return déplacement;
      }
   }
}
