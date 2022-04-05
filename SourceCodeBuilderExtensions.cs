using CommonBasicLibraries.CollectionClasses;
using Microsoft.CodeAnalysis;

namespace CommonSourceGeneratorsHelpers;
public static class SourceCodeBuilderExtensions
{
    //i like the idea of doing as extensions and not requiring new members for the interface.
    public static IWriter CustomExceptionLine(this IWriter w, Action<IWriter> message)
    {
        w.Write("throw new ")
        .BasicProcessesWrite()
            .Write("CustomBasicException($") //this will allow string interperlation (so if hints are needed for the errors, can be done).
            .AppendDoubleQuote(message)
            .Write(");");
        return w;
    }
    public static IWriter BasicProcessesWrite(this IWriter w)
    {
        w.GlobalWrite()
            .Write("CommonBasicLibraries.BasicDataSettingsAndProcesses.");
        return w;
    }
    public static IWriter SingleCollectionInfoWrite(this IWriter w, string genericType)
    {
        w.GlobalWrite()
            .Write("CommonBasicLibraries.CollectionClasses.BasicList")
            .SingleGenericWrite(genericType);
        return w;
    }
    public static IWriter SingleGenericWrite(this IWriter w, string genericType)
    {
        w.Write("<")
            .Write(genericType)
            .Write(">");
        return w;
    }
    public static IWriter SingleGenericWrite(this IWriter w, Action<IWriter> action) //this means you could have extras.
    {
        w.Write("<");
        action.Invoke(w);
        w.Write(">");
        return w;
    }
    //this means this can be as complex as you want (i think).
    public static IWriter CustomGenericWrite(this IWriter w, Action<IWriter> action) //this means you could have extras.
    {
        w.SingleGenericWrite(action);
        return w; //give me a choice.
    }
    public static IWriter GlobalWrite(this IWriter w)
    {
        w.Write("global::");
        return w;
    }
    public static IWriter SystemWrite(this IWriter w)
    {
        w.GlobalWrite()
            .Write("System.");
        return w;
    }
    public static IWriter EmptyEqualsEndString(this IWriter w)
    {
        w.Write(" = ")
            .AppendDoubleQuote(w => w.Write(""))
            .Write(";");
        return w;
    }
    public static ICodeBlock InitializeNewBasicList(this ICodeBlock w, BasicList<string> list, Action<IWriter> variableAction)
    {
        w.WriteLine(w =>
        {
            w.Write("global::CommonBasicLibraries.CollectionClasses.BasicList<string> ");
            variableAction.Invoke(w);
            w.Write(" = new()");
        }).WriteCodeBlock(w =>
        {
            int count = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                w.WriteLine(w =>
                {
                    w.AppendDoubleQuote(list[i]);
                    if (i != count - 1)
                    {
                        w.Write(",");
                    }
                });
            }
        }, endSemi: true);
        return w;
    }
    public static ICodeBlock InitializeNewBasicList(this ICodeBlock w, BasicList<string> list, string variableName)
    {
        return w.InitializeNewBasicList(list, w =>
        {
            w.Write(variableName);
        });
    }
    public static ICodeBlock InitializeNewBasicList(this ICodeBlock w, BasicList<int> list, Action<IWriter> variableAction)
    {
        w.WriteLine(w =>
        {
            w.Write("global::CommonBasicLibraries.CollectionClasses.BasicList<int> ");
            variableAction.Invoke(w);
            w.Write(" = new()");
        }).WriteCodeBlock(w =>
        {
            int count = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                w.WriteLine(w =>
                {
                    w.Write(list[i]);
                    if (i != count - 1)
                    {
                        w.Write(",");
                    }
                });
            }
        }, endSemi: true);
        return w;
    }
    public static ICodeBlock InitializeNewBasicList(this ICodeBlock w, BasicList<int> list, string variableName)
    {
        return w.InitializeNewBasicList(list, w =>
        {
            w.Write(variableName);
        });
    }
    public static void InitializeFromCustomList<T>(this ICodeBlock w, BasicList<T> list, Action<IWriter, T> action)
    {
        int count = list.Count;
        for (int i = 0; i < list.Count; i++)
        {
            w.WriteLine(w =>
            {
                action.Invoke(w, list[i]);
                if (i != count - 1)
                {
                    w.Write(",");
                }
            });
        }
    }
    public static void InitializeFromCustomList<T>(this IWriter w, BasicList<T> list, Action<IWriter, T> action)
    {
        //don't assume anything.
        //iwriter means will be same line.
        int count = list.Count;
        for (int i = 0; i < list.Count; i++)
        {
            action.Invoke(w, list[i]);
            if (i != count - 1)
            {
                w.Write(",");
            }
        }
    }
    public static IWriter SymbolFullNameWrite(this IWriter w, INamedTypeSymbol symbol)
    {
        w.GlobalWrite()
            .Write(symbol.ContainingNamespace)
            .Write(".")
            .Write(symbol.Name);
        return w;
    }
    public static IWriter EndsWithEmptyString(this IWriter w)
    {
        w.AppendDoubleQuote()
            .Write(";");
        return w;
    }
    public static void StartPartialClass(this SourceCodeStringBuilder builder, INamedTypeSymbol symbol, Action<ICodeBlock> action)
    {
        builder.WriteLine("#nullable enable")
                .WriteLine(w =>
                {
                    w.Write("namespace ")
                    .Write(symbol.ContainingNamespace)
                    .Write(";");
                })
            .WriteLine(w =>
            {
                w.Write("public partial class ")
                .Write(symbol.Name);
            })
            .WriteCodeBlock(w =>
            {
                action.Invoke(w);
            });
    }
    public static void StartPartialClassImplements(this SourceCodeStringBuilder builder, INamedTypeSymbol symbol, Action<IWriter> implements, Action<ICodeBlock> content) //sometimes, you need to implement something. i think name should be a little different.
    {
        builder.WriteLine("#nullable enable")
                .WriteLine(w =>
                {
                    w.Write("namespace ")
                    .Write(symbol.ContainingNamespace)
                    .Write(";");
                })
            .WriteLine(w =>
            {
                w.Write("public partial class ")
                .Write(symbol.Name);
                implements.Invoke(w);
            })
            .WriteCodeBlock(w =>
            {
                content.Invoke(w);
            });
    }
    public static void StartGlobalProcesses(this SourceCodeStringBuilder builder, Compilation compilation, string finishNamespace, string className, Action<ICodeBlock> action)
    {
        string ns = compilation.AssemblyName!;
        builder.WriteLine("#nullable enable")
        .WriteLine(w =>
        {
            w.Write("namespace ")
            .Write(ns)
            .Write(".")
            .Write(finishNamespace)
            .Write(";");
        })
        .WriteLine(w =>
        {
            w.Write("public static partial class ")
            .Write(className);
        })
        .WriteCodeBlock(w =>
        {
            action.Invoke(w);
        });
    }
    public static void StartGlobalProcesses(this SourceCodeStringBuilder builder, Compilation compilation, string className, Action<ICodeBlock> action)
    {
        string ns = compilation.AssemblyName!;
        builder.WriteLine("#nullable enable")
        .WriteLine(w =>
        {
            w.Write("namespace ")
            .Write(ns)
            .Write(";");
        })
        .WriteLine(w =>
        {
            w.Write("public static partial class ")
            .Write(className);
        })
        .WriteCodeBlock(w =>
        {
            action.Invoke(w);
        });
    }
    public static void StartInternalGlobalProcesses(this SourceCodeStringBuilder builder, Compilation compilation, string className, Action<ICodeBlock> action)
    {
        string ns = compilation.AssemblyName!;
        builder.WriteLine("#nullable enable")
        .WriteLine(w =>
        {
            w.Write("namespace ")
            .Write(ns)
            .Write(";");
        })
        .WriteLine(w =>
        {
            w.Write("internal static partial class ")
            .Write(className);
        })
        .WriteCodeBlock(w =>
        {
            action.Invoke(w);
        });
    }
    public static void StartInternalGlobalProcesses(this SourceCodeStringBuilder builder, Compilation compilation, string finishNamespace, string className, Action<ICodeBlock> action)
    {
        string ns = compilation.AssemblyName!;
        builder.WriteLine("#nullable enable")
        .WriteLine(w =>
        {
            w.Write("namespace ")
            .Write(ns)
            .Write(".")
            .Write(finishNamespace)
            .Write(";");
        })
        .WriteLine(w =>
        {
            w.Write("internal static partial class ")
            .Write(className);
        })
        .WriteCodeBlock(w =>
        {
            action.Invoke(w);
        });
    }
    public static void StartGlobalProcesses(this SourceCodeStringBuilder builder, Compilation compilation, string finishNamespace, string className, string methodName, Action<ICodeBlock> action)
    {
        string ns = compilation.AssemblyName!;
        builder.WriteLine("#nullable enable")
        .WriteLine(w =>
        {
            w.Write("namespace ")
            .Write(ns)
            .Write(".")
            .Write(finishNamespace)
            .Write(";");
        })
        .WriteLine(w =>
        {
            w.Write("public static partial class ")
            .Write(className);
        })
        .WriteCodeBlock(w =>
        {
            w.WriteLine(w =>
            {
                w.Write("public static void ")
                .Write(methodName)
                .Write("()");
            })
            .WriteCodeBlock(w =>
            {
                action.Invoke(w);
            });
        });
    }
    public static IWriter BasicListWrite(this IWriter w)
    {
        w.GlobalWrite()
            .Write("CommonBasicLibraries.CollectionClasses.BasicList");
        return w; //this will be the full basiclist namespace so even if not referenced, will work.  since this is common enough.
    }
}