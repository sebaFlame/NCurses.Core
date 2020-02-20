using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;
using JetBrains.dotMemoryUnit;
using JetBrains.dotMemoryUnit.Kernel;

using NCurses.Core.Tests.Model;
using NCurses.Core.Interop.SafeHandles;

[assembly: SuppressXUnitOutputException]

namespace NCurses.Core.Tests
{
    public class GCTest
    {
        public GCTest(ITestOutputHelper testOutputHelper)
        {
            DotMemoryUnitTestOutput.SetOutputMethod((string str) => testOutputHelper.WriteLine(str));
        }

        //new method to scope the IWindow instance
        private void InitDisposalTestInternal(out Type stdScrType)
        {
            using (IWindow window = NCurses.Start())
            {
                stdScrType = window.GetType();

                window.Write("test");
            }
        }

        [DotMemoryUnit(CollectAllocations = true)]
        [Fact]
        public void InitDisposalTest()
        {
            MemoryCheckPoint beforeTestCheckPoint = dotMemory.Check();

            this.InitDisposalTestInternal(out Type stdScrType);
            GC.Collect();

            dotMemory.Check(memory =>
            {
                Assert.Equal(0, memory.GetDifference(beforeTestCheckPoint).GetNewObjects(obj => obj.Type.Is(stdScrType)).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(beforeTestCheckPoint).GetNewObjects(obj => obj.Type.Is(
                    typeof(NewWindowSafeHandle), 
                    typeof(WindowSafeHandle), 
                    typeof(StdScrSafeHandle))).ObjectsCount);
            });
        }

        private void InitFinalizerTestInternal(out Type stdScrType, out Type windowType)
        {
            IWindow stdScr = NCurses.Start();
            stdScrType = stdScr.GetType();

            IWindow window = NCurses.CreateWindow();
            windowType = window.GetType();
        }

        [DotMemoryUnit(CollectAllocations = true)]
        [Fact]
        public void InitFinalizerTest()
        {
            MemoryCheckPoint beforeInitCheckPoint = dotMemory.Check();

            this.InitFinalizerTestInternal(out Type stdScrType, out Type windowType);
            GC.Collect();

            //Created window (including safehandle) should've been collected through finalizer
            MemoryCheckPoint afterFinalizerCheckPoint = dotMemory.Check(memory =>
            {
                Assert.Equal(1, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(stdScrType)).ObjectsCount);
                Assert.Equal(1, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(typeof(StdScrSafeHandle))).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(windowType)).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(
                    typeof(NewWindowSafeHandle),
                    typeof(WindowSafeHandle))).ObjectsCount);
            });

            NCurses.End();
            GC.Collect();

            dotMemory.Check(memory =>
            {
                Assert.Equal(0, memory.GetDifference(afterFinalizerCheckPoint).GetNewObjects(obj => obj.Type.Is(stdScrType)).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(afterFinalizerCheckPoint).GetNewObjects(obj => obj.Type.Is(
                    typeof(NewWindowSafeHandle),
                    typeof(WindowSafeHandle),
                    typeof(StdScrSafeHandle))).ObjectsCount);
            });
        }

        private void SubWindowDisposalTestInternal(out Type stdScrType, out Type windowType)
        {
            using (IWindow stdScr = NCurses.Start())
            {
                stdScrType = stdScr.GetType();

                using (IWindow parentWindow = NCurses.CreateWindow())
                {
                    windowType = parentWindow.GetType();

                    IWindow win1 = parentWindow.SubWindow(20, 20, 0, 0);
                    IWindow win2 = parentWindow.SubWindow(20, 20, 0, 0);

                    win1.Write("test");
                    win2.Write("test");

                    win1.Dispose();
                    win2.Dispose();
                }
            }
        }

        [DotMemoryUnit(CollectAllocations = true)]
        [Fact]
        public void SubWindowDisposalTest()
        {
            MemoryCheckPoint beforeTestCheckPoint = dotMemory.Check();

            this.SubWindowDisposalTestInternal(out Type stdScrType, out Type windowType);
            GC.Collect();

            dotMemory.Check(memory =>
            {
                Assert.Equal(0, memory.GetDifference(beforeTestCheckPoint).GetNewObjects(obj => obj.Type.Is(stdScrType, windowType)).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(beforeTestCheckPoint).GetNewObjects(obj => obj.Type.Is(
                    typeof(NewWindowSafeHandle),
                    typeof(WindowSafeHandle),
                    typeof(StdScrSafeHandle))).ObjectsCount);
            });
        }

        private void SubWindowFinalizerTestInternal(out Type stdScrType, out Type windowType)
        {
            IWindow stdScr = NCurses.Start();
            stdScrType = stdScr.GetType();

            IWindow parentWindow = NCurses.CreateWindow();
            windowType = parentWindow.GetType();

            IWindow win1 = parentWindow.SubWindow(20, 20, 0, 0);
            IWindow win2 = parentWindow.SubWindow(20, 20, 0, 0);

            win1.Write("test");
            win2.Write("test");
        }

        [DotMemoryUnit(CollectAllocations = true)]
        [Fact]
        public void SubWindowFinalizerTest()
        {
            MemoryCheckPoint beforeInitCheckPoint = dotMemory.Check();

            this.SubWindowFinalizerTestInternal(out Type stdScrType, out Type windowType);
            GC.Collect();

            MemoryCheckPoint afterFinalizerCheckPoint = dotMemory.Check(memory =>
            {
                Assert.Equal(1, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(stdScrType)).ObjectsCount);
                Assert.Equal(1, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(typeof(StdScrSafeHandle))).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(windowType)).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(
                    typeof(NewWindowSafeHandle),
                    typeof(WindowSafeHandle))).ObjectsCount);
            });

            NCurses.End();
            GC.Collect();

            dotMemory.Check(memory =>
            {
                Assert.Equal(0, memory.GetDifference(afterFinalizerCheckPoint).GetNewObjects(obj => obj.Type.Is(stdScrType)).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(afterFinalizerCheckPoint).GetNewObjects(obj => obj.Type.Is(
                    typeof(NewWindowSafeHandle),
                    typeof(WindowSafeHandle),
                    typeof(StdScrSafeHandle))).ObjectsCount);
            });
        }

        private void PadDisposalTestInternal(out Type stdScrType, out Type windowType, out Type padType)
        {
            using (IWindow stdScr = NCurses.Start())
            {
                stdScrType = stdScr.GetType();

                using (IPad pad = NCurses.CreatePad(200, 200))
                {
                    padType = pad.GetType();

                    pad.Write("test");
                }

                using (IWindow parentWindow = NCurses.CreateWindow())
                {
                    windowType = parentWindow.GetType();

                    using(IPad pad = NCurses.CreatePad(parentWindow, 200, 200))
                    {
                        pad.Write("test");
                    }
                }
            }
        }

        [DotMemoryUnit(CollectAllocations = true)]
        [Fact]
        public void PadDisposalTest()
        {
            MemoryCheckPoint beforeTestCheckPoint = dotMemory.Check();

            this.PadDisposalTestInternal(out Type stdScrType, out Type windowType, out Type padType);
            GC.Collect();

            dotMemory.Check(memory =>
            {
                Assert.Equal(0, memory.GetDifference(beforeTestCheckPoint).GetNewObjects(obj => obj.Type.Is(stdScrType, windowType, padType)).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(beforeTestCheckPoint).GetNewObjects(obj => obj.Type.Is(
                    typeof(NewWindowSafeHandle),
                    typeof(WindowSafeHandle),
                    typeof(StdScrSafeHandle))).ObjectsCount);
            });
        }

        private void PadFinalizerTestInternal(out Type stdScrType, out Type windowType, out Type padType)
        {
            IWindow stdScr = NCurses.Start();
            stdScrType = stdScr.GetType();

            IPad pad1 = NCurses.CreatePad(200, 200);
            padType = pad1.GetType();
            pad1.Write("test");

            IWindow parentWindow = NCurses.CreateWindow();
            windowType = parentWindow.GetType();
            IPad pad2 = NCurses.CreatePad(parentWindow, 200, 200);
            pad2.Write("test");
        }

        [DotMemoryUnit(CollectAllocations = true)]
        [Fact]
        public void PadFinalizerTest()
        {
            MemoryCheckPoint beforeInitCheckPoint = dotMemory.Check();

            this.PadFinalizerTestInternal(out Type stdScrType, out Type windowType, out Type padType);
            GC.Collect();

            MemoryCheckPoint afterFinalizerCheckPoint = dotMemory.Check(memory =>
            {
                Assert.Equal(1, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(stdScrType)).ObjectsCount);
                Assert.Equal(1, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(typeof(StdScrSafeHandle))).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(windowType, padType)).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(
                    typeof(NewWindowSafeHandle),
                    typeof(WindowSafeHandle))).ObjectsCount);
            });

            NCurses.End();
            GC.Collect();

            dotMemory.Check(memory =>
            {
                Assert.Equal(0, memory.GetDifference(afterFinalizerCheckPoint).GetNewObjects(obj => obj.Type.Is(stdScrType)).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(afterFinalizerCheckPoint).GetNewObjects(obj => obj.Type.Is(
                    typeof(NewWindowSafeHandle),
                    typeof(WindowSafeHandle),
                    typeof(StdScrSafeHandle))).ObjectsCount);
            });
        }

        private void PanelDisposalTestInternal(out Type stdScrType, out Type windowType, out Type panelType)
        {
            using (IWindow stdScr = NCurses.Start())
            {
                stdScrType = stdScr.GetType();
                
                IWindow win1, win2, win3, win4;
                win1 = NCurses.CreateWindow(20, 20, 0, 0);
                win2 = NCurses.CreateWindow(20, 20, 0, 0);
                win3 = NCurses.CreateWindow(20, 20, 0, 0);
                win4 = NCurses.CreateWindow(20, 20, 0, 0);

                windowType = win1.GetType();

                IPanel panel1, panel2, panel3, resultPanel;
                panel1 = NCurses.CreatePanel(win1);
                panel2 = NCurses.CreatePanel(win2);
                panel3 = NCurses.CreatePanel(win3);

                panelType = panel1.GetType();

                resultPanel = panel2.Below();
                resultPanel.Dispose();

                resultPanel = panel1.Above();
                resultPanel.Dispose();

                panel3.WrappedWindow = win4;

                panel1.Dispose();
                panel2.Dispose();
                panel3.Dispose();

                win1.Dispose();
                win2.Dispose();
                win3.Dispose();
                win4.Dispose();
            }
        }

        [DotMemoryUnit(CollectAllocations = true)]
        [Fact]
        public void PanelDisposalTest()
        {
            MemoryCheckPoint beforeTestCheckPoint = dotMemory.Check();

            this.PanelDisposalTestInternal(out Type stdScrType, out Type windowType, out Type panelType);
            GC.Collect();

            dotMemory.Check(memory =>
            {
                Assert.Equal(0, memory.GetDifference(beforeTestCheckPoint).GetNewObjects(obj => obj.Type.Is(stdScrType, windowType, panelType)).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(beforeTestCheckPoint).GetNewObjects(obj => obj.Type.Is(
                    typeof(NewWindowSafeHandle),
                    typeof(WindowSafeHandle),
                    typeof(StdScrSafeHandle),
                    typeof(PanelSafeHandle),
                    typeof(NewPanelSafeHandle))).ObjectsCount);
            });
        }

        private void PanelFinalizerTestInternal(out Type stdScrType, out Type windowType, out Type panelType)
        {
            IWindow stdScr = NCurses.Start();

            stdScrType = stdScr.GetType();

            IWindow win1, win2, win3, win4;
            win1 = NCurses.CreateWindow(20, 20, 0, 0);
            win2 = NCurses.CreateWindow(20, 20, 0, 0);
            win3 = NCurses.CreateWindow(20, 20, 0, 0);
            win4 = NCurses.CreateWindow(20, 20, 0, 0);

            windowType = win1.GetType();

            IPanel panel1, panel2, panel3, resultPanel;
            panel1 = NCurses.CreatePanel(win1);
            panel2 = NCurses.CreatePanel(win2);
            panel3 = NCurses.CreatePanel(win3);

            panelType = panel1.GetType();

            resultPanel = panel2.Below();
            resultPanel = panel1.Above();

            panel3.WrappedWindow = win4;
        }

        [DotMemoryUnit(CollectAllocations = true)]
        [Fact]
        public void PanelFinalizerTest()
        {
            MemoryCheckPoint beforeInitCheckPoint = dotMemory.Check();

            this.PanelFinalizerTestInternal(out Type stdScrType, out Type windowType, out Type panelType);
            GC.Collect();

            MemoryCheckPoint afterFinalizerCheckPoint = dotMemory.Check(memory =>
            {
                Assert.Equal(1, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(stdScrType)).ObjectsCount);
                Assert.Equal(1, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(typeof(StdScrSafeHandle))).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(windowType, panelType)).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(
                    typeof(NewWindowSafeHandle),
                    typeof(WindowSafeHandle),
                    typeof(NewPanelSafeHandle),
                    typeof(PanelSafeHandle))).ObjectsCount);
            });

            NCurses.End();
            GC.Collect();

            dotMemory.Check(memory =>
            {
                Assert.Equal(0, memory.GetDifference(afterFinalizerCheckPoint).GetNewObjects(obj => obj.Type.Is(stdScrType)).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(afterFinalizerCheckPoint).GetNewObjects(obj => obj.Type.Is(
                    typeof(NewWindowSafeHandle),
                    typeof(WindowSafeHandle),
                    typeof(StdScrSafeHandle))).ObjectsCount);
            });
        }

        private void PanelFromSubWindowDisposalTestInternal(out Type stdScrType, out Type windowType, out Type panelType)
        {
            using (IWindow stdScr = NCurses.Start())
            {
                stdScrType = stdScr.GetType();

                using (IWindow parentWindow = NCurses.CreateWindow())
                {
                    IWindow win1, win2, win3, win4;
                    win1 = parentWindow.SubWindow(20, 20, 0, 0);
                    win2 = parentWindow.SubWindow(20, 20, 0, 0);
                    win3 = parentWindow.SubWindow(20, 20, 0, 0);
                    win4 = parentWindow.SubWindow(20, 20, 0, 0);

                    windowType = win1.GetType();

                    IPanel panel1, panel2, panel3, resultPanel;
                    panel1 = NCurses.CreatePanel(win1);
                    panel2 = NCurses.CreatePanel(win2);
                    panel3 = NCurses.CreatePanel(win3);

                    panelType = panel1.GetType();

                    resultPanel = panel2.Below();
                    resultPanel.Dispose();

                    resultPanel = panel1.Above();
                    resultPanel.Dispose();

                    panel3.WrappedWindow = win4;

                    panel1.Dispose();
                    panel2.Dispose();
                    panel3.Dispose();

                    win1.Dispose();
                    win2.Dispose();
                    win3.Dispose();
                    win4.Dispose();
                }
            }
        }

        [DotMemoryUnit(CollectAllocations = true)]
        [Fact]
        public void PanelFromSubWindowDisposalTest()
        {
            MemoryCheckPoint beforeTestCheckPoint = dotMemory.Check();

            this.PanelFromSubWindowDisposalTestInternal(out Type stdScrType, out Type windowType, out Type panelType);
            GC.Collect();

            dotMemory.Check(memory =>
            {
                Assert.Equal(0, memory.GetDifference(beforeTestCheckPoint).GetNewObjects(obj => obj.Type.Is(stdScrType, windowType, panelType)).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(beforeTestCheckPoint).GetNewObjects(obj => obj.Type.Is(
                    typeof(NewWindowSafeHandle),
                    typeof(WindowSafeHandle),
                    typeof(StdScrSafeHandle),
                    typeof(PanelSafeHandle),
                    typeof(NewPanelSafeHandle))).ObjectsCount);
            });
        }

        private void PanelFromSubWindowFinalizerTestInternal(out Type stdScrType, out Type windowType, out Type panelType)
        {
            IWindow stdScr = NCurses.Start();

            stdScrType = stdScr.GetType();

            IWindow parentWindow = NCurses.CreateWindow();

            IWindow win1, win2, win3, win4;
            win1 = parentWindow.SubWindow(20, 20, 0, 0);
            win2 = parentWindow.SubWindow(20, 20, 0, 0);
            win3 = parentWindow.SubWindow(20, 20, 0, 0);
            win4 = parentWindow.SubWindow(20, 20, 0, 0);

            windowType = win1.GetType();

            IPanel panel1, panel2, panel3, resultPanel;
            panel1 = NCurses.CreatePanel(win1);
            panel2 = NCurses.CreatePanel(win2);
            panel3 = NCurses.CreatePanel(win3);

            panelType = panel1.GetType();

            resultPanel = panel2.Below();
            resultPanel = panel1.Above();

            panel3.WrappedWindow = win4;
        }

        [DotMemoryUnit(CollectAllocations = true)]
        [Fact]
        public void PanelFromSubWindowFinalizerTest()
        {
            MemoryCheckPoint beforeInitCheckPoint = dotMemory.Check();

            this.PanelFromSubWindowFinalizerTestInternal(out Type stdScrType, out Type windowType, out Type panelType);
            GC.Collect();

            MemoryCheckPoint afterFinalizerCheckPoint = dotMemory.Check(memory =>
            {
                Assert.Equal(1, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(stdScrType)).ObjectsCount);
                Assert.Equal(1, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(typeof(StdScrSafeHandle))).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(windowType, panelType)).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(beforeInitCheckPoint).GetNewObjects(obj => obj.Type.Is(
                    typeof(NewWindowSafeHandle),
                    typeof(WindowSafeHandle),
                    typeof(NewPanelSafeHandle),
                    typeof(PanelSafeHandle))).ObjectsCount);
            });

            NCurses.End();
            GC.Collect();

            dotMemory.Check(memory =>
            {
                Assert.Equal(0, memory.GetDifference(afterFinalizerCheckPoint).GetNewObjects(obj => obj.Type.Is(stdScrType)).ObjectsCount);
                Assert.Equal(0, memory.GetDifference(afterFinalizerCheckPoint).GetNewObjects(obj => obj.Type.Is(
                    typeof(NewWindowSafeHandle),
                    typeof(WindowSafeHandle),
                    typeof(StdScrSafeHandle))).ObjectsCount);
            });
        }

        [DotMemoryUnit(CollectAllocations = true)]
        [Fact]
        public void AllocationTest()
        {
            string testString = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

            using (IWindow stdScr = NCurses.Start())
            {
                using (IPad pad = NCurses.CreatePad(stdScr, 2000, 2000))
                {
                    MemoryCheckPoint beforeWriteCheckPoint = dotMemory.Check();

                    for (int i = 0; i < 2000; i++)
                    {
                        pad.Write(testString);
                    }

                    stdScr.Refresh();

                    dotMemory.Check(memory =>
                    {
                        Assert.True(memory.GetTrafficFrom(beforeWriteCheckPoint).AllocatedMemory.SizeInBytes <= 264000); //2000 * testString
                    });
                }
            }
        }
    }
}