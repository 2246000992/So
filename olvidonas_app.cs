using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace JuegoCartas
{
    public partial class FormJuego : Form
    {
        private string carpetaCartas = "cartas";
        private string carpetaUsadas = "usadas";
        private List<string> cartasDisponibles;
        private List<string> cartasUsadas;
        private string cartaActual;
        private Random random;

        public FormJuego()
        {
            InitializeComponent();
            InicializarJuego();

            // Agregar el controlador de eventos FormClosing
            this.FormClosing += FormJuego_FormClosing;
        }

        private void InicializarJuego()
        {
            // Obtener todas las cartas disponibles en la carpeta "cartas"
            string[] archivosCartas = Directory.GetFiles(carpetaCartas);
            cartasDisponibles = new List<string>(archivosCartas);
            cartasUsadas = new List<string>();

            // Crear una instancia de Random
            random = new Random();

            // Cambiar el texto y el controlador de eventos del botón
            btnComenzar.Text = "Comenzar";
            btnComenzar.Click += BtnComenzar_Click;
        }

        private void BtnComenzar_Click(object sender, EventArgs e)
        {
            // Restaurar las cartas disponibles si no hay ninguna
            if (!cartasDisponibles.Any())
            {
                RestaurarCartas();
                return;
            }

            // Cambiar el texto y el controlador de eventos del botón
            btnComenzar.Text = "Siguiente";
            btnComenzar.Click -= BtnComenzar_Click;
            btnComenzar.Click += BtnSiguiente_Click;

            // Obtener una carta aleatoria de las disponibles
            if (cartasDisponibles.Count > 0)
            {
                int indiceCarta = random.Next(0, cartasDisponibles.Count);
                cartaActual = cartasDisponibles[indiceCarta];

                // Mostrar la carta actual en la vista
                pictureBoxCarta.ImageLocation = cartaActual;
            }
        }

        private void BtnSiguiente_Click(object sender, EventArgs e)
        {
            if (cartasDisponibles.Any())
            {
                // Mover la carta actual a la carpeta "usadas"
                string rutaCartaUsada = Path.Combine(carpetaUsadas, Path.GetFileName(cartaActual));
                File.Move(cartaActual, rutaCartaUsada);

                // Agregar la carta actual a la lista de cartas usadas
                cartasUsadas.Add(cartaActual);

                // Eliminar la carta actual de las cartas disponibles
                cartasDisponibles.Remove(cartaActual);

                // Obtener una carta aleatoria de las disponibles (si hay alguna)
                if (cartasDisponibles.Count > 0)
                {
                    int indiceCarta = random.Next(0, cartasDisponibles.Count);
                    cartaActual = cartasDisponibles[indiceCarta];

                    // Mostrar la carta actual en la vista
                    pictureBoxCarta.ImageLocation = cartaActual;
                }
            }
            else
            {
                MessageBox.Show("No hay más cartas disponibles.");
                MoverCartasUsadasACartas();
            }
        }

        private void MoverCartasUsadasACartas()
        {
            // Mover todas las cartas usadas a la carpeta "cartas"
            foreach (var cartaUsada in cartasUsadas)
            {
                string rutaCarta = Path.Combine(carpetaCartas, Path.GetFileName(cartaUsada));
                File.Move(cartaUsada, rutaCarta);
            }

            // Actualizar la lista de cartas disponibles
            string[] archivosCartas = Directory.GetFiles(carpetaCartas);
            cartasDisponibles = new List<string>(archivosCartas);
            cartasUsadas.Clear();
        }

        private void FormJuego_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Restaurar las cartas antes de cerrar la aplicación
            RestaurarCartas();
        }

        private void RestaurarCartas()
        {
            // Mover todas las cartas usadas a la carpeta "cartas"
            foreach (var cartaUsada in cartasUsadas)
            {
                string rutaCarta = Path.Combine(carpetaCartas, Path.GetFileName(cartaUsada));
                File.Move(cartaUsada, rutaCarta);
            }

            // Actualizar la lista de cartas disponibles
            string[] archivosCartas = Directory.GetFiles(carpetaCartas);
            cartasDisponibles = new List<string>(archivosCartas);
            cartasUsadas.Clear();
        }
    }
}
