# MDA: Sequence Diagram 2 State Machine Conversion

### First checkout instructions

1. Install ReSharper (license may be found in EGP Dropbox)
2. Clone repository
3. Compile
4. Switch to <solution-root>\packages\Machine.Specifications.0.8.2\tools and execute InstallResharperRunner.8.2.bat
5. Have fun

### Frameworks used

1. **Testing:** [Machine.Specifications](https://github.com/machine/machine.specifications)
2. **Mocking:** Rhino.Mocks / [Machine.Fakes](https://github.com/BjRo/Machine.Fakes)
3. **CLI:** [CommandLineParser](https://commandline.codeplex.com/)
4. **IoC:** [Ninject](http://www.codeproject.com/Articles/680159/Implementing-Dependency-Injection-using-Ninject)

### Before pushing

Remember to **always** do a R# Code Reformat on **all** files edited prior to pushing! (VS shortcut: Ctrl+E+C).