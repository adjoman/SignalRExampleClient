using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRSample;

public partial class MainPage : ContentPage
{
	private readonly HubConnection _connection;

	public MainPage()
	{
		InitializeComponent();

		BindingContext = this;

		_connection = new HubConnectionBuilder()
			.WithUrl("http://10.0.0.220:5175/chat")
			.WithAutomaticReconnect()
			.Build();

		_connection.On<string>("MessageReceived", (message) =>
		{
            //chatMessages.Text += $"{Environment.NewLine}{message}";
            //chatMessages.Text += message;
			ChangeLabel(message);
        });

		Task.Run(() =>
		{
			Dispatcher.Dispatch(async () =>
				await _connection.StartAsync());
		});

		chatMessages.Text = "Start from Code";
	}

	async void ChangeLabel(string _message)
	{
        MainThread.BeginInvokeOnMainThread(() =>
        {
            this.Title = _message;
            chatMessages.Text = _message;
        });        
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		try
		{
			await _connection.InvokeCoreAsync("SendMessage", args: new[]
			{ myChatMessage.Text });

			myChatMessage.Text = string.Empty;
		}
		catch (Exception ex)
		{
			string test = string.Empty;
		}
	}
}


