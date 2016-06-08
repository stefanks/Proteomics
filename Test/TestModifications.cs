// Copyright 2012, 2013, 2014 Derek J. Bailey
// Modified work copyright 2016 Stefan Solntsev
// 
// This file (TestFragments.cs) is part of Proteomics.
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

using NUnit.Framework;
using Proteomics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    [TestFixture]
    public sealed class TestModifications
    {

        [Test]
        public void NameAndSites()
        {
            // Empty modification, has no name and by default has Sites = ModificationSites.Any
            Modification a = new Modification();
            Modification b = new Modification(a);
            Assert.AreEqual(" (Any)", b.NameAndSites);
        }

        [Test]
        public void ModificationEquality()
        {
            // Empty modification, has no name and by default has Sites = ModificationSites.Any
            Modification a = new Modification();
            Modification b = new Modification();
            Modification c = new Modification(0, "c");
            Modification d = new Modification(0, "", ModificationSites.E);
            Assert.IsTrue(a.Equals(b));
            Assert.IsFalse(a.Equals(c));
            Assert.IsFalse(a.Equals(d));
        }
    }
}