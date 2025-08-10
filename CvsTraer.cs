using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerene_App
{
    public enum TipoPregunta
    {
        OpcionMultiple,
        Abierta,
        Desconocido
    }

    public class OpcionRespuesta
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public override string ToString() => Texto;
    }

    public class Pregunta
    {
        public int Numero { get; set; }
        public string Texto { get; set; }
        public TipoPregunta Tipo { get; set; }
        public Seccion? Seccion { get; set; }
        public bool Multiple { get; set; }
        public List<OpcionRespuesta> Opciones { get; set; } = new();
        public OpcionRespuesta? RespuestaCorrecta { get; set; }
        public string OpcionesResumen => string.Join(" | ", Opciones.Select(o => o.Texto));
    }


    
}
