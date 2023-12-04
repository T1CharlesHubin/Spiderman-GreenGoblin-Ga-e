using SFML;
using SFML.Audio;
using SFML.System;
using SFML.Graphics;
using SFML.Window;
using AS2023_Prototype;
using static AS2023_Prototype.Utilitaires;
using System.Diagnostics;

Random générateur = new();

int compteurTours = 1;

Texture textureBombe = new("../../../Image/pumpkinbomb.jpg");
Texture textureExplosion = new("../../../Image/Explosion.jpg");

Bombe[] bombes = CréerBombes(5, new Point(-1, -1), new Sprite(textureBombe));

Objet[,] carte = PartieÉtudiante.CréerCarte(NB_CASES_LARGE, NB_CASES_HAUT, NB_BÂTIMENTS, générateur);

Point posSpiderMan = GénérerPosition(générateur, carte, Objet.Bâtiment);
Point posGobelinVert = GénérerPosition(générateur);
Sprite spriteFondEcran = new(new Texture("../../../Image/Ciel.jpg"));
Sprite spriteVictoire = new(new Texture("../../../Image/Victoire.jpg"));
Sprite spritePerdu = new(new Texture("../../../Image/Defaite.jpg"));
Sprite spriteMainChar = new(new Texture("../../../Image/Spider-Man.jpg"));
Sprite spriteVilain = new(new Texture("../../../Image/GreenGoblin.jpg"));
Texture textureBatiment = new("../../../Image/Batiment.png");
Sprite[] spriteBatiment = new Sprite[NB_CASES_LARGE * NB_CASES_HAUT];

Texture textureChampForce = new("../../../Image/ChampForce.png");
Sprite[] spriteChampForce = new Sprite[NB_CASES_LARGE * NB_CASES_HAUT];


Remplir(spriteBatiment, textureBatiment);
Remplir(spriteChampForce, textureChampForce);

RenderWindow window = new(new VideoMode(LARGEUR_CARTE_PIXEL, HAUTEUR_CARTE_PIXEL), "SFML window");
window.Closed += new EventHandler(OnClose);
window.KeyPressed += OnKeyPressed;

bool perdu = false;
bool gagné = false;

bool wait = true;

//window.KeyPressed += new EventHandler<KeyEventArgs>(OnInput);
bool exploseCeFrame = false;
// Start the game loop
while (window.IsOpen && !perdu && !gagné)
{
    spriteMainChar.Position = new SFML.System.Vector2f(posSpiderMan.X * TAILLE_CASE, posSpiderMan.Y * TAILLE_CASE);
    spriteVilain.Position = new SFML.System.Vector2f(posGobelinVert.X * TAILLE_CASE, posGobelinVert.Y * TAILLE_CASE);

    window.DispatchEvents();
    window.Clear();

    GestionAffichage();

    if (!wait)
    {
        AfficherBombes2(bombes);
        GestionExplosion(exploseCeFrame, ref bombes, textureBombe);
        MiseÀJour();

        if (exploseCeFrame)
        {
            perdu = ExploserBombes(bombes);
        }
        if (PartieÉtudiante.EstObjetRecherché(carte, posSpiderMan, Objet.Force))
        {
            perdu = true;
        }


        ++compteurTours;
    }

    AfficherBombes(bombes);
    GérerFinPartie();
    window.Display();
    wait = true;
}
//window.Display();
Console.WriteLine("Terminé. Pressez <enter> pour fermer le programme");
Console.ReadLine();

/////////////////////////////
static void GestionExplosion(bool doitExploser, ref Bombe[] bombes, Texture texture)
{
    if (doitExploser)
    {
        bombes = CréerBombes(10, new Point(-1, -1), new Sprite(texture));
    }
}


void MiseÀJour()
{
    DéplacementSpiderMan déplacement = ConvertirToucheEnDéplacement();
    posSpiderMan = PartieÉtudiante.DéplacerSpiderMan(déplacement, carte, posSpiderMan);
    posGobelinVert = PartieÉtudiante.DéplacerGobelinVert(carte, posGobelinVert, générateur);
    exploseCeFrame = PartieÉtudiante.AttaqueGobelin(compteurTours, posSpiderMan, bombes);
}

void GestionAffichage()
{
    window.Draw(spriteFondEcran);
    Afficher(carte);
    if(wait)
    {
        AfficherBombes(bombes);
    }
    window.Draw(spriteMainChar);
    window.Draw(spriteVilain);
   // GérerFinPartie();
}

void GérerFinPartie()
{
    if (gagné = EstVictoireHéro()) // note : affectation volontaire
    {
        window.Draw(spriteVictoire);
    }
    else if (perdu)
    {
        window.Draw(spritePerdu);
    }
}

bool EstVictoireHéro()
{
    return PartieÉtudiante.SontÉgaux(posSpiderMan, posGobelinVert);
}

void AfficherBombes(Bombe[] bombes)
{
    for (int i = 0; i < bombes.Length; i++)
    {
        if (PartieÉtudiante.EstPositionValide(bombes[i].Position))
        {
            window.Draw(bombes[i].Image);
        }
    }
}
void AfficherBombes2(Bombe[] bombes)
{
    for (int i = 0; i < bombes.Length; i++)
    {
        if (PartieÉtudiante.EstPositionValide(bombes[i].Position))
        {
            window.Draw(bombes[i].Image);
            bombes[i].Image = new Sprite(textureBombe);
            bombes[i].Image.Position = new SFML.System.Vector2f(bombes[i].Position.X * TAILLE_CASE, bombes[i].Position.Y * TAILLE_CASE);
        }
    }
}
bool ExploserBombes(Bombe[] bombes)
{
    const int TailleExplosion = 2;
    bool perdu = false;
    for (int i = 0; i < bombes.Length; i++)
    {
        bombes[i].Image = new Sprite(textureExplosion);
        bombes[i].Image.Position = new SFML.System.Vector2f((bombes[i].Position.X - 1) * TAILLE_CASE, (bombes[i].Position.Y - 1) * TAILLE_CASE);
        if (PartieÉtudiante.Distance(bombes[i].Position, posSpiderMan) < TailleExplosion)
        {
            perdu = true;
        }
    }
    return perdu;
}

void Afficher(Objet[,] carte)
{
    int nbColonnes = carte.GetLength(0);
    int nbLignes = carte.GetLength(1);

    for (int i = 0; i < nbColonnes; ++i)
    {
        for (int j = 0; j < nbLignes; ++j)
        {
            switch (carte[i, j])
            {
                case Objet.Force:
                    spriteChampForce[i * nbLignes + j].Position = new SFML.System.Vector2f(i * TAILLE_CASE, j * TAILLE_CASE);
                    window.Draw(spriteChampForce[i * nbLignes + j]);
                    break;
                case Objet.Bâtiment:
                    spriteBatiment[i * nbLignes + j].Position = new SFML.System.Vector2f(i * TAILLE_CASE, j * TAILLE_CASE);
                    window.Draw(spriteBatiment[i * nbLignes + j]);
                    break;
                default:
                    break;
            }
        }
    }
}
void OnKeyPressed(object? sender, KeyEventArgs e)
{
    wait = false;
}
static void OnClose(object sender, EventArgs e)
{
    // Close the window when OnClose event is received
    RenderWindow window = (RenderWindow)sender;
    window.Close();
}
static void Remplir(Sprite[] sprites, Texture texture)
{
    int taille = sprites.GetLength(0);
    for (int i = 0; i < taille; ++i)
    {
        sprites[i] = new Sprite(texture);
    }
}

DéplacementSpiderMan ConvertirToucheEnDéplacement()
{
    DéplacementSpiderMan déplacement;
    if (Keyboard.IsKeyPressed(Keyboard.Key.A))
    {
        déplacement = DéplacementSpiderMan.GrimpeGauche;
    }
    else if (Keyboard.IsKeyPressed(Keyboard.Key.W))
    {
        déplacement = DéplacementSpiderMan.GrimpeHaut;
    }
    else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
    {
        déplacement = DéplacementSpiderMan.GrimpeDroit;
    }
    else if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
    {
        déplacement = DéplacementSpiderMan.ToileDroite;
    }
    else if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
    {
        déplacement = DéplacementSpiderMan.ToileGauche;
    }
    else if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
    {
        déplacement = DéplacementSpiderMan.ToileHaut;
    }
    else
    {
        déplacement = DéplacementSpiderMan.Chute;
    }
    return déplacement;
}