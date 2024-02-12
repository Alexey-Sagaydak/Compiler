using ICSharpCode.AvalonEdit;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Compiler;

public sealed class AvalonEditBehaviour : Behavior<TextEditor>
{
    public static readonly DependencyProperty InputTextProperty =
        DependencyProperty.Register("InputText", typeof(string), typeof(AvalonEditBehaviour),
        new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

    public string InputText
    {
        get { return (string)GetValue(InputTextProperty); }
        set { SetValue(InputTextProperty, value); }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject != null)
        {
            AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        if (AssociatedObject != null)
        {
            AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
        }
    }

    private void AssociatedObjectOnTextChanged(object sender, EventArgs eventArgs)
    {
        var textEditor = sender as TextEditor;
        if (textEditor != null)
        {
            if (textEditor.Document != null)
            {
                InputText = textEditor.Document.Text;
            }
        }
    }
}