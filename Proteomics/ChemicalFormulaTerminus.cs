using System;
using Chemistry;

namespace Proteomics
{
    public class ChemicalFormulaTerminus : IHasChemicalFormula
    {
        public ChemicalFormulaTerminus(string v)
        {
            thisChemicalFormula = new ChemicalFormula(v);
        }

        public double MonoisotopicMass
        {
            get
            {
                return thisChemicalFormula.MonoisotopicMass;
            }
        }

        public ChemicalFormula thisChemicalFormula
        {
            get; private set;
        }
    }
}