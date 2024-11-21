using System.Formats.Tar;

namespace HomeFoods.CustomControls;

public partial class OutlinedEntryControl : Grid
{
    public OutlinedEntryControl()
    {
        InitializeComponent();
    }


    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        propertyName: nameof(Text),
        returnType: typeof(string),
        declaringType: typeof(OutlinedEntryControl),
        defaultValue: null,
        defaultBindingMode: BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set { SetValue(TextProperty, value); }
    }

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
      propertyName: nameof(Placeholder),
      returnType: typeof(string),
      declaringType: typeof(OutlinedEntryControl),
      defaultValue: null,
      defaultBindingMode: BindingMode.OneWay);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set { SetValue(PlaceholderProperty, value); }
    }

    private void txtEntry_Focused(object sender, FocusEventArgs e)
    {
        TextUp();
    }

    private void txtEntry_Unfocused(object sender, FocusEventArgs e)
    {
        PlaceholderMove();
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        txtEntry.Focus();
    }

    private void txtEntry_Loaded(object sender, EventArgs e)
    {
        PlaceholderMove();
    }

    private void PlaceholderMove()
    {
        if (!string.IsNullOrWhiteSpace(Text))
        {
            TextUp();
        }
        else
        {
            lblPlaceholder.FontSize = 15;
            lblPlaceholder.TranslateTo(0, 0, 80, easing: Easing.Linear);
        }
    }
    private void TextUp()
    {
        lblPlaceholder.FontSize = 15;
        lblPlaceholder.TranslateTo(0, -25, 80, easing: Easing.Linear);
    }
}