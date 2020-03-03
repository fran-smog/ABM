using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABM
{
    public partial class FormAlumno : Form
    {
        BDEntities contexto;
        public FormAlumno()
        {
            InitializeComponent();
        }
        private void LimpiarTextos()
        {
            TxtidAlumno.Text = "";
            txtNombre.Text = "";
            txtApaterno.Text = "";
            txtAmaterno.Text = "";
            txtRutaImagen.Text = "";
            pbFotografia.ImageLocation = "";

        }
        private void LlenarGrid()
        {
            try
            {
                contexto = new BDEntities();
                var datos = from a in contexto.Alumnoes
                            select new
                            {
                                Nombre = a.nombre,
                                paterno=a.ApellidoPaterno,
                                materno=a.ApellidoMaterno
                            };
                dgvDatos.DataSource = datos.ToList();
                dgvDatos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {

                MessageBox.Show("ocurrio un error:" +ex.ToString());
            }
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                contexto = new BDEntities();

                //Creamos un nuevo alumno
                Alumno alumno = new Alumno()
                {
                    IdAlumno = int.Parse(TxtidAlumno.Text),
                    nombre = txtNombre.Text,
                    ApellidoPaterno = txtApaterno.Text,
                    ApellidoMaterno = txtAmaterno.Text,
                    Fotografia = txtRutaImagen.Text,
                };

                //Agregar el objeto a la BD
                contexto.Alumnoes.Add(alumno);
                contexto.SaveChanges();
                MessageBox.Show("alumno agregado correctamente");
                LimpiarTextos();
                LlenarGrid();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Ocurrio un error: " + ex.ToString());
            }
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Title = "Abrir Imagen";
                openFile.Filter = "archivos JPG(*.jpg,*jpeg)| *.jpg; *.jpeg";
                if (openFile.ShowDialog()==DialogResult.OK)
                {
                    string image = openFile.FileName;
                    txtRutaImagen.Text = image;
                    pbFotografia.Image = Image.FromFile(image);
                }
                else
                {
                    MessageBox.Show("No se Selecciono Imagen");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error: " + ex.ToString());
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtidAlumno.Text != string.Empty)
                {
                    int clave = int.Parse(TxtidAlumno.Text);
                    contexto = new BDEntities();

                    Alumno buscar = (from a in contexto.Alumnoes
                                     where a.IdAlumno == clave
                                     select a).SingleOrDefault();
                    if (buscar != null)
                    {
                        txtNombre.Text = buscar.nombre;
                        txtApaterno.Text = buscar.ApellidoPaterno;
                        txtAmaterno.Text = buscar.ApellidoMaterno;
                        txtRutaImagen.Text = buscar.Fotografia;
                        pbFotografia.Image = Image.FromFile(buscar.Fotografia);
                    }
                    else
                    {
                        LimpiarTextos();
                        MessageBox.Show("no se encontro el registro");
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Ocurrio un error: " + ex.ToString());
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtidAlumno.Text != string.Empty)
                {
                    int clave = int.Parse(TxtidAlumno.Text);
                    contexto = new BDEntities();
                    Alumno eliminar = (from a in contexto.Alumnoes
                                       where a.IdAlumno == clave
                                       select a).SingleOrDefault();

                    if (eliminar != null)
                    {
                        contexto.Alumnoes.Remove(eliminar);
                        contexto.SaveChanges();
                        MessageBox.Show("Registro eliminado");
                        LlenarGrid();
                    }
                    else
                    {
                        MessageBox.Show("No se encontro el registro");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Ocurrio un error: " + ex.ToString());
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtidAlumno!=null)
                {
                    int clave = int.Parse(TxtidAlumno.Text);
                    contexto = new BDEntities();
                    Alumno actualizar = (from a in contexto.Alumnoes
                                         where a.IdAlumno == clave
                                         select a).SingleOrDefault();
                    if (actualizar!=null)
                    {
                        actualizar.nombre = txtNombre.Text;
                        actualizar.ApellidoMaterno = txtAmaterno.Text;
                        actualizar.ApellidoPaterno = txtApaterno.Text;
                        actualizar.Fotografia = txtRutaImagen.Text;

                        contexto.SaveChanges();
                        MessageBox.Show("Regitro actualizado");
                        LimpiarTextos();
                        LlenarGrid();
                    }
                    else
                    {
                        MessageBox.Show("No se encontro registro");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Ocurrio un error: " + ex.ToString());
            }
        }

        private void FormAlumno_Load(object sender, EventArgs e)
        {
            LlenarGrid();
        }
    }
}
