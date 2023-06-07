using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominios; 

namespace negocio
{
    public class PokemonNegocio
    {

        public List<Pokemon> listar()
        {
            List<Pokemon> lista= new List<Pokemon>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.Id from POKEMONS P, ELEMENTOS E, ELEMENTOS D where e.Id=p.IdTipo and D.Id=p.IdDebilidad and activo=1";
                comando.Connection = conexion;  
                conexion.Open();
                lector = comando.ExecuteReader();
                while(lector.Read())
                {
                    Pokemon aux= new Pokemon();
                    aux.Numero = lector.GetInt32(0);
                    aux.Id = (int)lector["Id"];
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];
                    if (!(lector["UrlImagen"] is DBNull))
                    aux.UrlImagen = (string)lector["UrlImagen"];
                    aux.Tipo = new Elemento();
                    int v = (int)lector["Id"];
                    aux.Tipo.Id = v;
                    aux.Tipo.Descripcion = (string)lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    int h= (int)lector["Id"];
                    aux.Debilidad.Id = h;
                    aux.Debilidad.Descripcion = (string)lector["Debilidad"];

                    lista.Add(aux);
                }
                conexion.Close();
                return lista;


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void agregar(Pokemon nuevo)
        {   AccesoDatos datos= new AccesoDatos();
            try
            {
                datos.setearConsulta("Insert into Pokemons (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen) values(" + nuevo.Numero + ",'" + nuevo.Nombre+"','" + nuevo.Descripcion+"', 1,@IdTipo,@IdDebilidad, @UrlImagen)");
                datos.setearParametro("@IdTipo", nuevo.Tipo.Id);
                datos.setearParametro("@IdDebilidad", nuevo.Debilidad.Id);
                datos.setearParametro("@UrlImagen", nuevo.UrlImagen);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void modificar(Pokemon pokemon)
        {   
            AccesoDatos datos= new AccesoDatos();
            try
            {
                datos.setearConsulta("Update POKEMONS set Numero = @num, Nombre= @nomb, Descripcion=@desc, UrlImagen=@img, IdTipo=@IdTipo, IdDebilidad=@IdDebilidad where Id=@Id ");
                datos.setearParametro("@num", pokemon.Numero);
                datos.setearParametro("@nomb",pokemon.Nombre);
                datos.setearParametro("@desc", pokemon.Descripcion);
                datos.setearParametro("@img",pokemon.UrlImagen);
                datos.setearParametro("@IdTipo",pokemon.Tipo.Id);
                datos.setearParametro("@IdDebilidad",pokemon.Debilidad.Id);
                datos.setearParametro("@Id",pokemon.Id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public List<Pokemon> filtrar (string campo, string criterio, string filtro)
        {
            List<Pokemon> lista = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "select Numero, Nombre, P.Descripcion, UrlImagen, e.Descripcion tipo, d.Descripcion debilidad, p.IdTipo, p.IdDebilidad, p.Id from pokemons p, elementos e,elementos d where e.Id = p.IdTipo and d.Id = p.IdDebilidad and p.Activo = 1 and ";
                if (campo == "Numero")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "numero > "+filtro;
                            break;   
                        case "Menor a":
                            consulta += "numero < " + filtro;
                            break;
                        default:
                            consulta += "numero = " + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "P.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "P.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "P.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Numero = datos.Lector.GetInt32(0);
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];
                    aux.Tipo = new Elemento();
                    int v = (int)datos.Lector["Id"];
                    aux.Tipo.Id = v;
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    int h = (int)datos.Lector["Id"];
                    aux.Debilidad.Id = h;
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from pokemons where id=@id");
                datos.setearParametro("id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void eliminarLogico(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("update POKEMONS set Activo = 0 Where id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
