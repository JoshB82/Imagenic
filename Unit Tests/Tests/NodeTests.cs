using Imagenic.Core.Utilities.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.UnitTests.Tests;

[TestClass]
public class NodeTests
{
    [TestMethod]
    public void CycleTest()
    {
        var node1 = new Node<string>();
        var node2 = new Node<int>();
        var node3 = new Node<float>();

        node1.Parent = node2;
        node2.Parent = node3;
        node3.Parent = node1;

        bool expectedOutcome = true;
        bool actualOutcome = node1.IsPartOfCycle();

        Assert.AreEqual(expectedOutcome, actualOutcome);
    }

    [TestMethod]
    public void CycleTest2()
    {
        var node1 = new Node<string>();
        var node2 = new Node<int>();
        var node3 = new Node<float>();

        node1.Parent = node2;
        node2.Parent = node3;
        node3.Parent = node2;

        bool expectedOutcome = false;
        bool actualOutcome = node1.IsPartOfCycle();

        Assert.AreEqual(expectedOutcome, actualOutcome);
    }
}