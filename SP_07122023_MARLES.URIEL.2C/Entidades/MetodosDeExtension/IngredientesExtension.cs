using Entidades.Enumerados;
using System;


namespace Entidades.MetodosDeExtension
{
    public static class IngredientesExtension
    {
        public static double CalcularCostoIngredientes(this List<EIngrediente> ingredientes,int costoinicial)
        {
            double costoAumentado = costoinicial;
            foreach(EIngrediente ingrediente in ingredientes) 
            {
                costoAumentado += costoinicial * (double)ingrediente / 100;
            }
            return costoAumentado;
        }

        public static List<EIngrediente> IngredientesAleatorios(this Random rand)
        {
            List<EIngrediente> ingredientes = new List<EIngrediente>()
            {
                EIngrediente.QUESO,
                EIngrediente.PANCETA,
                EIngrediente.ADHERESO,
                EIngrediente.HUEVO,
                EIngrediente.JAMON};
            int numeroAleatorio = rand.Next(1, ingredientes.Count + 1);
            return ingredientes.Take(numeroAleatorio).ToList();
        }

    }
}
