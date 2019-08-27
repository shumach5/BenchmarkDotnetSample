using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace BenchmarkDotnetSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var switcher = new BenchmarkSwitcher(new[]
            {
            typeof(StructVsClassBenchmark)
        });

            args = new string[] { "0" };
            switcher.Run(args);
        }
    }

    public class Config : ManualConfig
    {
        public Config()
        {
            Add(MarkdownExporter.GitHub); 
            Add(MemoryDiagnoser.Default);
            Add(Job.ShortRun); //It takes so long without this
            
        }
    }

    [Config(typeof(Config))]
    public class StructVsClassBenchmark
    {
        SampleClass[] sampleClasses;
        SampleStruct[] sampleStructs;
        int _arrayNum = 100000;

        [GlobalSetup]
        public void Setup()
        {
            sampleClasses = Enumerable.Range(1, _arrayNum).Select(item => new SampleClass { Age = item  }).ToArray();
            sampleStructs = Enumerable.Range(1, _arrayNum).Select(item => new SampleStruct { Age = item }).ToArray();
        }

        [Benchmark(Baseline = true)]
        public void SumOfClass()
        {
            int sum=0;
            for (int i = 0; i < sampleClasses.Length; i++)
            {
                sum += sampleClasses[i].Age;
            }
        }

        [Benchmark]
        public void SumOfSturuct()
        {
            int sum = 0;
            for (int i = 0; i < sampleStructs.Length; i++)
            {
                sum += sampleStructs[i].Age;
            }
        }      

        [GlobalCleanup]
        public void Cleanup()
        {
            //WriteSomething
        }

        class SampleClass
        {
            public int Age { get; set; }
        }

        struct SampleStruct
        {
            public int Age { get; set; }
        }
    }
}
