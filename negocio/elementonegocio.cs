using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using dominios;

namespace negocio
{
    public class elementonegocio
    {
        public List<Elemento> Listar()
        {   List<Elemento> lista=new List<Elemento> ();
            AccesoDatos datos = new AccesoDatos ();
            try
            {   
                datos.setearConsulta("select Id, Descripcion from Elementos");
                datos.ejecutarLectura();
                while(datos.Lector.Read())
                {
                    Elemento aux = new Elemento ();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    lista.Add(aux);
                }
                return lista;
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
    }
}
