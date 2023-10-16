using ASCHM.Context;
using ASCHM.Models;
using Oracle.ManagedDataAccess.Client;

namespace ASCHM.Repository
{
    public class Operation : IOperation
    {
        #region Constructor
        private IHRMDBContext _hrmdbContext;
        public Operation(IHRMDBContext hrmdbContext)
        {
            _hrmdbContext = hrmdbContext;
        }
        #endregion

        #region Data
        public List<Solicitud> GetSolicitudesPendientes
        {
            get
            {
                List<Solicitud> lista = new List<Solicitud>();
                using (OracleConnection con = _hrmdbContext.GetConn())
                {
                    using (OracleCommand cmd = _hrmdbContext.GetCommand())
                    {
                        try
                        {
                            con.Open();
                            cmd.BindByName = true;
                            cmd.CommandText = "select * from hmetro.datos_sc_abierta_sect";
                            OracleDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                lista.Add(new Solicitud()
                                {
                                    Id = reader["CD_SOL_COM"].ToString(),
                                    Fecha = reader["DT_SOL_COM"].ToString(),
                                    Solicitante = reader["NM_SOLICITANTE"].ToString(),
                                    Cod = reader["CD_SETOR"].ToString(),
                                    Sector = reader["NM_SETOR"].ToString(),
                                    Situacion = reader["TP_SITUACAO"].ToString(),
                                    Observacion = reader["DS_OBSERVACAO"].ToString(),
                                    UsuarioAutorizador = reader["CD_USUARIO"].ToString(),
                                    Nivel = reader["NIVEL"].ToString(),
                                    UrlSolicitud = reader["REPORTE"].ToString(),
                                    UrlDatoAdj = reader["LINK"].ToString(),

                                });
                            }
                            reader.Dispose();
                            return lista;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }
        }

        public List<NivelAuth> GetNivelAuth
        {
            get
            {
                List<NivelAuth> lista = new List<NivelAuth>();
                using (OracleConnection con = _hrmdbContext.GetConn())
                {
                    using (OracleCommand cmd = _hrmdbContext.GetCommand())
                    {
                        try
                        {
                            con.Open();
                            cmd.BindByName = true;
                            cmd.CommandText = "select * from hmetro.sc_estado_autoriz";
                            OracleDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                lista.Add(new NivelAuth()
                                {
                                    Id = reader["CD_SOL_COM"].ToString(),
                                    Nivel = reader["CD_NIVEL_AUT_SOLIC"].ToString(),
                                    Nombre = reader["DS_NIVEL_AUT_SOLIC"].ToString(),
                                    Usuario = reader["CD_USUARIO"].ToString(),
                                    Estado = reader["USUARIO_AUTORIZA"].ToString(),
                                    Fecha = reader["FECHA_AUTORIZA"].ToString(),
                                });
                            }
                            reader.Dispose();
                            return lista;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }
        }

        public List<Motivo> GetMotivo
        {
            get
            {
                List<Motivo> lista = new List<Motivo>();
                using (OracleConnection con = _hrmdbContext.GetConn())
                {
                    using (OracleCommand cmd = _hrmdbContext.GetCommand())
                    {
                        try
                        {
                            con.Open();
                            cmd.BindByName = true;
                            cmd.CommandText = "select * from hmetro.mot_cancel_sol";
                            OracleDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                lista.Add(new Motivo()
                                {
                                    Id = reader["COD_MOTIVO"].ToString(),
                                    Opcion = reader["MOTIVO"].ToString()
                                });
                            }
                            reader.Dispose();
                            return lista;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }
        }

        public List<Historial> GetHistorial
        {
            get
            {
                List<Historial> lista = new List<Historial>();
                using (OracleConnection con = _hrmdbContext.GetConn())
                {
                    using (OracleCommand cmd = _hrmdbContext.GetCommand())
                    {
                        try
                        {
                            con.Open();
                            cmd.BindByName = true;
                            cmd.CommandText = "select * from hmetro.datos_sc_autorizada";
                            OracleDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                lista.Add(new Historial()
                                {
                                    Id = reader["CD_SOL_COM"].ToString(),
                                    Fecha = reader["DT_SOL_COM"].ToString(),
                                    UsuarioAutorizador = reader["CD_USUARIO"].ToString(),
                                    Cod = reader["CD_SETOR"].ToString(),
                                    Sector = reader["NM_SETOR"].ToString(),
                                    Observacion = reader["DS_OBSERVACAO"].ToString(),
                                    Estado = reader["TP_SITUACAO"].ToString(),
                                    Nivel = reader["NIVEL"].ToString()
                                });
                            }
                            reader.Dispose();
                            return lista;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }
        }

        #endregion

        #region Stored Procedures
        public void Autorizar(string usuario, int id, int nivel)
        {
            try
            {
                
                using (OracleConnection con = _hrmdbContext.GetConn())
                {
                    con.Open();
                    OracleCommand cmd = new OracleCommand("HMETRO.PR_ITG_ACT_SC", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new OracleParameter("pc_usuario", OracleDbType.Varchar2, System.Data.ParameterDirection.Input)).Value = usuario;
                    cmd.Parameters.Add(new OracleParameter("pn_ord_comp", OracleDbType.Int32, System.Data.ParameterDirection.Input)).Value = id;
                    cmd.Parameters.Add(new OracleParameter("pn_nivel", OracleDbType.Int32, System.Data.ParameterDirection.Input)).Value = nivel;
                    cmd.Parameters.Add(new OracleParameter("pc_error", OracleDbType.Varchar2, System.Data.ParameterDirection.Output));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Algo no funcionó bien!", ex);
            }
        }

        public void Cancelar(string usuario, int id, int nivel, int motivo, string observacion)
        {
            try
            {
               
                using (OracleConnection con = _hrmdbContext.GetConn())
                {
                    con.Open();
                    OracleCommand cmd = new OracleCommand("HMETRO.PR_ITG_CANC_SC", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new OracleParameter("cd_usuario", OracleDbType.Varchar2, System.Data.ParameterDirection.Input)).Value = usuario;
                    cmd.Parameters.Add(new OracleParameter("cd_sol_com", OracleDbType.Decimal, System.Data.ParameterDirection.Input)).Value = id;
                    cmd.Parameters.Add(new OracleParameter("nivel", OracleDbType.Decimal, System.Data.ParameterDirection.Input)).Value = nivel;
                    cmd.Parameters.Add(new OracleParameter("cod_motivo", OracleDbType.Int32, System.Data.ParameterDirection.Input)).Value = motivo;
                    cmd.Parameters.Add(new OracleParameter("observacion", OracleDbType.Varchar2, System.Data.ParameterDirection.Input)).Value = observacion;
                    cmd.Parameters.Add(new OracleParameter("pc_error", OracleDbType.Varchar2, System.Data.ParameterDirection.Output));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Algo no funcionó bien!", ex);
            }
        }

        #endregion

        public List<Autorizador> GetAutorizador
        {
            get
            {
                List<Autorizador> lista = new List<Autorizador>();
                using (OracleConnection con = _hrmdbContext.GetConn())
                {
                    using (OracleCommand cmd = _hrmdbContext.GetCommand())
                    {
                        try
                        {
                            con.Open();
                            cmd.BindByName = true;
                            cmd.CommandText = "select * from hmetro.autorizadores_sc";
                            OracleDataReader reader = cmd.ExecuteReader();



                            while (reader.Read())
                            {
                                lista.Add(new Autorizador()
                                {
                                   AutorizadorU = reader["CD_USUARIO"].ToString(),
                                });
                            }
                            reader.Dispose();
                            return lista;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }
        }

      
    }
}
