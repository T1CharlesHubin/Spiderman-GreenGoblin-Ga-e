using System.IO.Compression;
using static AS2023_Prototype.Utilitaires;

namespace AS2023_Prototype
{
   internal class PartieÉtudiante
   {
      //
      // ProchainePositionGobelin(pos,dir)
      // Intrants : pos (position initiale du Gobelin)
      //            dir (direction du déplacement)
      // Extrant : la nouvelle position du Gobelin
      // Note : cette fonction ne valide pas la nouvelle
      //        position du Gobelin, alors il est possible
      //        que celle-ci soit hors de la carte
      //
      public static Point ProchainePositionGobelin(Point pos, DéplacementGobelinVert dir)
      {
            int xInitialGobelin = pos.X;
            int yInitialGobelin = pos.Y;
            int xSuivantGobelin = xInitialGobelin;
            int ySuivantGobelin = yInitialGobelin;
            int[] mouvement = TrouverMouvementGobelinVert(dir);
            ySuivantGobelin = yInitialGobelin + mouvement[1];
            xSuivantGobelin = xInitialGobelin + mouvement[0];
            Point nouvellePositionGobelin = new Point(xSuivantGobelin, ySuivantGobelin);
            return nouvellePositionGobelin;
      }

      //
      // EstPositionValide(pos)
      // Intrants : pos (position)
      // Extrant : true seulement si pos est valide
      // Note : valide signifie que ni la coordonnée X, ni
      //        la coordonnée Y ne vaut -1
      //
      public static bool EstPositionValide(Point position)
      {
            return (position.X >= 0 && position.Y >= 0);
      }

      //
      // CréerCarte(largeur, hauteur, nbBâtiments, générateur)
      // Intrants : largeur (largeur de la carte, en cases)
      //            hauteur (hauteur de la carte, en cases)
      //            nbBâtiments (nombre de bâtiments à générer)
      //            générateur (source de hasard)
      // Extrant : tableau 2D de Objet contenant la carte générée
      // Détails :
      // - cette fonction doit, dans l'ordre :
      // -- créer une carte de la dimension demandée 
      // -- remplir ce tableau de Objet.Vide 
      // -- initialiser les bordures de la carte avec Objet.Force 
      // -- placer les bâtiments sur la carte
      // - enfin, retourner la carte dûment remplie 
      //
      public static Objet[,] CréerCarte(int largeur, int hauteur, int nbBâtiments, Random générateur)
      {
            Objet[,] carte = new Objet[largeur, hauteur];
            InitialiserTableau(carte, Objet.Vide);
            RemplirBordures(carte, Objet.Force);
            PlacerBâtiments(carte, nbBâtiments, générateur);
            return carte;
      }

      //
      // InitialiserTableau(carte, init)
      // Intrants : carte (tableau 2D de Objet)
      //            init (valeur initiale pour remplir la carte)
      // Extrant : s/o
      // Détails :
      // - cette fonction doit remplir chaque case de la carte
      //   avec la valeur init
      //
      public static void InitialiserTableau(Objet[,] carte, Objet init)
      {
            int hauteur = carte.GetLength(1);
            int largeur = carte.GetLength(0);
            for (int i = 0; i < largeur;i++)
            {
                for (int j = 0; j < hauteur;j++)
                {
                    carte[i, j] = init;
                }
            }
        }

      //
      // PlacerBâtiments(carte, nbBâtiments, générateur)
      // Intrants : carte (tableau 2D de Objet)
      //            nbBâtiments (nombre de bâtiments à générer)
      //            générateur (source de hasard)
      // Extrant : s/o
      // Détails :
      // - cette fonction doit placer nbBâtiments bâtiments sur
      //   la carte
      // - chaque bâtiment doit avoir une hauteur choisie de
      //   manière pseudoaléatoire sans déborder de la carte
      // - chaque bâtiment doit être sur une colonne distincte
      //   des autres
      // Note : un bâtiment ne doit pas être placé sur la bordure
      //        de la carte
      //
      public static void PlacerBâtiments(Objet[,] carte, int nbBâtiments, Random générateur)
      {
            // hauteurDepartBatiment = carte.GetLength(1) - 2, parce que -1 sera toujours la bordure
            int hauteurDepartBatiment = carte.GetLength(1) - 2;
            int NbBatimentsPoses = 0;
            while (NbBatimentsPoses  < nbBâtiments)
            {
                int emplacementBatiment = générateur.Next(1, carte.GetLength(0));
                int hauteurBatiment = générateur.Next(1, carte.GetLength(1) - 1);
                if (carte[emplacementBatiment, hauteurDepartBatiment] == Objet.Vide)
                {
                    for (int j = hauteurDepartBatiment; j > hauteurBatiment; j--)
                    {
                        carte[emplacementBatiment, j] = Objet.Bâtiment;
                    }
                    NbBatimentsPoses++;
                }
            }
      }

      //
      // RemplirBordures(carte, init)
      // Intrants : carte (tableau 2D de Objet)
      //            init (valeur initiale pour remplir la carte)
      // Extrant : s/o
      // Détails :
      // - cette fonction doit remplir la bordure de la carte
      //   avec la valeur init
      //
      public static void RemplirBordures(Objet[,] carte, Objet init)
      {
            int hauteur = carte.GetLength(1);
            int largeur = carte.GetLength(0);
            for (int i = 0; i < largeur ;i++)
            {
                for (int j = 0; j < hauteur;j++)
                {
                    if (i == 0 || i == largeur - 1 || j == 0 || j == hauteur - 1)
                    {
                        carte[i, j] = init;
                    }

                }
            }
        }
      //
      // EstObjetRecherché(carte, pos, cible)
      // Intrants : carte (tableau 2D de Objet)
      //            pos (position à examiner)
      //            cible (Objet souhaité)
      // Extrant : true seulement si la case examinée est dans
      //           la carte (bordures incluses) et contient
      //           l'Objet souhaité
      //
      public static bool EstObjetRecherché(Objet[,] carte, Point pos, Objet cible)
      {
            return (carte[pos.X, pos.Y] == cible);
      }

      //
      // DéplacerSpiderMan(choix, carte, pos)
      // Intrants : choix (déplacement que SpiderMan tente de faire)
      //            carte (tableau 2D de Objet)
      //            pos (position initiale de SpiderMan)
      // Extrant : le Point où se trouvera SpiderMan après déplacement
      // Détails :
      // - dans le cas des déplacements où SpiderMan grimpe (vers
      //   le haut, la gauche ou la droite), cette fonction tente
      //   d'abord d'appliquer le déplacement souhaité sur bâtiment,
      //   et si ce déplacement est refusé, SpiderMan chute (voir
      //   plus bas)
      // - grimper vers le bas est équivalent à une chute
      // - dans les cas de déplacements utilisant la toile (vers
      //   le haut, la gauche ou la droite), cette fonction calcule
      //   le delta du déplacement et tente le déplacement résultant
      //   sur bâtiment. Si ce déplacement est refusé, SpiderMan
      //   reste au même endroit
      // - une chute déplace SpiderMan d'une case vers le bas, sans
      //   sortir de la carte ou aller sur la bordure
      //
      public static Point DéplacerSpiderMan(DéplacementSpiderMan choix, Objet[,] carte, Point position)
      {
            Point posFinale = position;
            int dx = 0;
            int dy = 0;
            bool deplacementRefuse = false;
            if (choix == DéplacementSpiderMan.GrimpeGauche || choix == DéplacementSpiderMan.ToileGauche)
            {
                dx = -1;
            }
            else if (choix == DéplacementSpiderMan.GrimpeDroit || choix == DéplacementSpiderMan.ToileDroite)
            {
                dx = 1;
            }
            else if (choix == DéplacementSpiderMan.GrimpeHaut || choix == DéplacementSpiderMan.ToileHaut)
            {
                dy = -1;
            }

            if ( choix == DéplacementSpiderMan.GrimpeGauche || choix == DéplacementSpiderMan.GrimpeHaut || choix == DéplacementSpiderMan.GrimpeDroit)
            {
                posFinale =  AppliquerDéplacementSurBâtiment(carte, position, dx, dy);
                if (posFinale == position)
                {
                    deplacementRefuse = true;
                }
                
            }
            else if (choix == DéplacementSpiderMan.ToileGauche || choix == DéplacementSpiderMan.ToileHaut || choix == DéplacementSpiderMan.ToileDroite)
            {
                int delta = CalculerDelta(carte, position, dx, dy);
                posFinale = AppliquerDéplacementSurBâtiment(carte, position, dx * delta, dy * delta);
            }

            if (choix == DéplacementSpiderMan.Chute || deplacementRefuse == true)
            {
                posFinale = AppliquerDéplacementSurVide(carte, position, dx, dy);
            }

            return posFinale;
        }
      //
      // AppliquerDéplacementSurVide(carte, pos, dx, dy)
      // Intrants : carte (tableau 2D de Objet)
      //            pos (position initiale)
      //            dx (delta X du déplacement)
      //            dy (delta Y du déplacement)
      // Extrant : le Point après déplacement
      // Détails :
      // - soit dest la position pos à laquelle on aurait appliqué
      //   le déplacement dx,dy
      // - si la case dest de la carte contient Objet.Vide, la
      //   position retournée sera celle juste en dessous de dest
      //   (on chute d'une case)
      //   sinon, la position retournée sera celle juste en dessous
      //   de pos (on chute d'une case)
      //
      public static Point AppliquerDéplacementSurVide(Objet[,] carte, Point position, int deltaX, int deltaY)
      {
            Point dest = new Point(position.X + deltaX, position.Y + deltaY);
            if (carte[dest.X, dest.Y] == Objet.Vide)
            {
                Point posFinale = new Point(dest.X, dest.Y + 1);
                return posFinale;
            }
            else
            {
                Point posFinale = new Point(position.X, position.Y + 1);
                return posFinale;
            }
      }

      //
      // AppliquerDéplacementSurBâtiment(carte, pos, dx, dy)
      // Intrants : carte (tableau 2D de Objet)
      //            pos (position initiale)
      //            dx (delta X du déplacement)
      //            dy (delta Y du déplacement)
      // Extrant : le Point après déplacement
      // Détails :
      // - soit dest la position pos à laquelle on aurait appliqué
      //   le déplacement dx,dy
      // - si la case dest de la carte contient Objet.Bâtiment, la
      //   position retournée sera dest (déplacement accepté)
      //   sinon, la position retournée sera pos (déplacement refusé)
      //
      public static Point AppliquerDéplacementSurBâtiment(Objet[,] carte, Point position, int deltaX, int deltaY)
      {
            Point dest = new Point(position.X + deltaX, position.Y + deltaY);
            if (carte[dest.X, dest.Y] == Objet.Bâtiment)
            {
                Point posFinale = new Point(dest.X, dest.Y);
                return posFinale;
            }
            else
            {
                return position;
            }
      }
      //
      // CalculerDelta(carte, pos, dx, dy)
      // Intrants : carte (tableau 2D de Objet)
      //            pos (position initiale)
      //            dx (delta X du déplacement)
      //            dy (delta Y du déplacement)
      // Extrant : le delta calculé
      // Détails :
      // - cette fonction examine les positions à partir de
      //   pos avec dx,dy * un delta qui démarre à 1 puis
      //   incrémente (donc 1, 2, 3, ...) jusqu'à ce qu'une
      //   case non-vide soit trouvée
      // - par exemple, si pos est (3,5), dx est 1 et dy est 0,
      //   alors cette fonction examinera les cases (3+1,5),
      //   (3+2,5), (3+3,5), (3+i,5)... où chaque i est un delta,
      //   ceci jusqu'à ce qu'une case non-vide soit trouvée
      // - la fonction retournera le delta de la première case
      //   non-vide rencontrée
      //
      public static int CalculerDelta(Objet[,] carte, Point position, int deltaX, int deltaY)
      {
            int delta = 1;
            while (carte[position.X + deltaX * delta, position.Y + deltaY * delta] == Objet.Vide)
            {
                delta++;
            }
            return delta;
      }
      //
      // DéplacerGobelinVert(carte, pos, générateur)
      // Intrants : carte (tableau 2D de Objet)
      //            pos (position initiale du Gobelin)
      //            générateur (source de hasard)
      // Extrant : le Point où se trouvera le Gobelin après déplacement
      // Détails :
      // - cette fonction doit trouver de manière pseudoaélatoire
      //   une case voisine de la position actuelle du Gobelin et
      //   la retourner
      // - le Gobelin peut aller partout sur la carte, sauf sur la
      //   bordure (rappel : la bordure est faite de cases contenant
      //   Objet.Force)
      //
      public static Point DéplacerGobelinVert(Objet[,] carte, Point posGobelinVert, Random générateur)
      {
            Point posFinale = posGobelinVert;
            int deltaX = 0;
            int deltaY = 0;
            int[] deplacementPossible = new int[] { -1, 1 };
            while (posFinale == posGobelinVert)
            {
                int index = générateur.Next(0, 2);

                int axeDeplacement = générateur.Next(0, 2);
                if (axeDeplacement == 0)
                {
                    deltaX = deplacementPossible[index];
                }
                else
                {
                    deltaY = deplacementPossible[index];
                }

                if (carte[posGobelinVert.X + deltaX, posGobelinVert.Y + deltaY] != Objet.Force)
                {
                    int xFinal = posGobelinVert.X + deltaX;
                    int yFinal = posGobelinVert.Y + deltaY;
                    posFinale = new Point(xFinal, yFinal);
                }
            }
            return posFinale;
      }
      //
      // AttaqueGobelin(tour, cible, bombes)
      // Intrants : tour (entier représentant le numéro du tour de jeu)
      //            cible (la position sur laquelle lancer une bombe)
      //            bombes (tableau 1D de Bombe)
      // Extrant : true seulement si une bombe doit exploser
      // Détails :
      // - cette fonction doit déterminer si une bombe doit être lancée
      //   et lancer cette bombe le cas échéant, et détermine aussi si
      //   une explosion doit survenir
      // - les règles vont comme suit. Sachant que les tours
      //   débutent à 1 :
      // -- il y a quatre tours entre chaque lancement de bombe
      // -- les bombes seront lancées au tour 3, 7, 11, 15, etc.
      // -- les bombes exploseront aux tours qui sont des multiples
      //    de 13
      // Note : pour lancer une bombe, appelez la fonction LancerBombe
      //        de Utilitaires
      public static bool AttaqueGobelin(int numeroTour, Point cible, Bombe[] bombes)
      {
            if (numeroTour % 4 == 3)
            {
                LancerBombe(cible, bombes);
            }
            return (numeroTour % 13 == 0);
      }
      //
      // SontÉgaux(p0, p1)
      // Intrants : p0 (un Point)
      //            p1 (un Point)
      // Extrant : true seulement si p0 et p1 sont représentent
      //           des coordonnées équivalentes (même X, même Y)
      //
      public static bool SontÉgaux(Point p1, Point p2)
      {
            return (p1.X == p2.X && p1.Y == p2.Y);
      }
      //
      // Distance(p0, p1)
      // Intrants : p0 (un Point)
      //            p1 (un Point)
      // Extrant : distance Euclidienne entre p0 et p1
      //
      public static double Distance(Point p1, Point p2)
      {
            double distance = Math.Sqrt(Math.Pow(Math.Abs(p1.X - p2.X), 2) + Math.Pow(Math.Abs(p1.Y - p2.Y), 2));
            return distance;
      }

      public static int[] TrouverMouvementGobelinVert(DéplacementGobelinVert dir)
      {
            // Morti: tu dois utiliser la force des enum ici, il se passe quoi si je change la valeur des enum
            int mouvementX = 0;
            int mouvementY = 0;
            int[] mouvement = new int[2];
            if (dir == DéplacementGobelinVert.Haut)
            {
                mouvementY = -1;
            }
            else if (dir == DéplacementGobelinVert.Bas)
            {
                mouvementY = 1;
            }
            else if (dir == DéplacementGobelinVert.Gauche)
            {
                mouvementY = -1;
            }
            else if (dir == DéplacementGobelinVert.Droite)
            {
                mouvementY = 1;
            }
            else
            {
                Console.WriteLine("Ce mouvement n existe pas");
            }
            mouvement[0] = mouvementX;
            mouvement[1] = mouvementY;
            return mouvement;
      }
   }
}
