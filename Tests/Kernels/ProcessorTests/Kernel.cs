﻿using System;
using Sys = Cosmos.System;
using Cosmos.TestRunner;
using Cosmos.System.Graphics;
using System.Text;
using Cosmos.System.ExtendedASCII;
using Cosmos.System.ScanMaps;
using Cosmos.Core;
using System.Runtime.InteropServices;
using Cosmos.HAL;

namespace ProcessorTests
{
    public class Kernel : Sys.Kernel
    {
        protected override void BeforeRun()
        {
            Console.WriteLine("Cosmos booted successfully. Starting Tests");
        }

        protected override void Run()
        {
            try
            {
                //TestVendorNameIsNotBlank();
                TestCycleCount();
                //TestCycleRateIsNotZero();
                
                TestController.Completed();
            }
            catch (Exception e)
            {
                mDebugger.Send("Exception occurred: " + e.Message);
                mDebugger.Send(e.Message);
                TestController.Failed();
            }
        }

        public void TestVendorNameIsNotBlank()
        {
            string vendorName = CPU.GetCPUVendorName();
            mDebugger.Send(vendorName);
            bool isVendorNameBlank = string.IsNullOrWhiteSpace(vendorName);
            mDebugger.Send("Vendor name: ");
            mDebugger.Send(vendorName);
            Assert.IsFalse(isVendorNameBlank, "Processor vendor name is blank.");
        }

        public void TestCycleCount()
        {
            ulong cycleCount = CPU.GetCPUUptime();
            mDebugger.Send("Cycle count: " + cycleCount);
            mDebugger.SendNumber(cycleCount);
            bool isCycleCountZero = cycleCount == 0;
            Assert.IsFalse(isCycleCountZero, "Processor cycle count is zero.");
            ulong secondCount = CPU.GetCPUUptime();
            mDebugger.Send("Cycle count: " + secondCount);
            mDebugger.SendNumber(secondCount);
            Assert.IsTrue(secondCount > cycleCount, "Processor cycle count is increasing");
        }

        public void TestCycleRateIsNotZero()
        {
            long cycleRate = CPU.GetCPUCycleSpeed();
            bool isCycleRateZero = cycleRate == 0;
            Assert.IsFalse(isCycleRateZero, "Processor cycle rate is zero.");
        }
    }
}
