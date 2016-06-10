// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work copyright 2016 Stefan Solntsev
// 
// This file (Isotopologue.cs) is part of Proteomics.
// 
// Proteomics is free software: you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Proteomics is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
// License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with Proteomics. If not, see <http://www.gnu.org/licenses/>.

using Chemistry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Proteomics
{
    public class ModificationWithMultiplePossibilities : Modification, IHasMass, IEnumerable<Modification>
    {
        private readonly SortedList<double, Modification> _modifications;

        public Modification this[int index]
        {
            get { return _modifications.Values[index]; }
        }

        public int Count
        {
            get { return _modifications.Count; }
        }
        
        public ModificationWithMultiplePossibilities(string name, ModificationSites sites = ModificationSites.None)
            : base(0, name, sites)
        {
            _modifications = new SortedList<double, Modification>();
        }

        public Modification AddModification(Modification modification)
        {
            if (!Sites.ContainsSite(modification.Sites))
            {
                throw new ArgumentException("Unable to add a modification with sites other than " + Sites);
            }
            _modifications.Add(modification.MonoisotopicMass, modification);
            MonoisotopicMass = _modifications.Keys.Average();
            return modification;
        }

        public bool Contains(Modification modification)
        {
            return _modifications.ContainsValue(modification);
        }

        /// <summary>
        /// Calculate the expected spacings between a group of Peptides (channels) in Th
        /// </summary>
        /// <param name="peptides">The peptides to calculate the spacings between</param>
        /// <param name="charge">The charge state of those peptides</param>
        /// <returns>An array of Th spacings between each peptide in increasing m/z</returns>
        public static double[] GetExpectedSpacings<T2>(IList<T2> peptides, int charge) where T2 : AminoAcidPolymer
        {
            if (peptides.Count <= 1)
            {
                throw new ArgumentOutOfRangeException("peptides", "Not enough peptides to calculate spacings");
            }

            // There is always 1 less spacings than peptides.
            double[] spacings = new double[peptides.Count - 1];

            // Ensure sorted order by mass
            T2[] sortedPeptides = peptides.OrderBy(p => p.MonoisotopicMass).ToArray();

            // Convert the first peptide to m/z space
            double previousMz = sortedPeptides[0].ToMz(charge);

            // Loop over each other peptide in sorted order
            for (int i = 1; i < sortedPeptides.Length; i++)
            {
                // Grab the current peptide's m/z
                double currentMZ = peptides[i].ToMz(charge);

                // Calculate the spacing between the two channels
                spacings[i - 1] = currentMZ - previousMz;

                // Save the current peptide m/z as the previous m/z
                previousMz = currentMZ;
            }
            return spacings;
        }

        public IEnumerator<Modification> GetEnumerator()
        {
            return _modifications.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _modifications.Values.GetEnumerator();
        }
    }
}