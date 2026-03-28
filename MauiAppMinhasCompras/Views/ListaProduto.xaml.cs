using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
	}

	protected async override void OnAppearing()
	{
		try
		{
			lista.Clear();

			List<Produto> tmp = await App.Db.Getall();

			tmp.ForEach(i => lista.Add(i));
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");

		}
	}

	private void ToolbarItem_Clicked(object sender, EventArgs e)
	{
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());

		}
		catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK");
		}
	}
	//search
	private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
	{
		try
		{
			string q = e.NewTextValue;

			lst_produtos.IsRefreshing = true;

			lista.Clear();

			List<Produto> tmp = await App.Db.Search(q);

			tmp.ForEach(i => lista.Add(i));
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
		finally
		{
			lst_produtos.IsRefreshing = false;
		}
	}
	//soma dos produtos
	private void ToolbarItem_Clicked_1(object sender, EventArgs e)
	{
		double soma = lista.Sum(i => i.Total);

		string msg = $"O total é {soma:C}";

		DisplayAlert("Total Dos Produtos", msg, "OK");
	}
	// excluir
	private async void MenuItem_Clicked_1(object sender, EventArgs e)
	{
		try
		{
			MenuItem selecionado = sender as MenuItem;

			Produto p = selecionado.BindingContext as Produto;

			bool confirm = await DisplayAlert("Tem certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

			if (confirm)
			{
				await App.Db.delete(p.Id);
				lista.Remove(p);
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
	}

	private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		try
		{
			Produto p = e.SelectedItem as Produto;

			Navigation.PushAsync(new Views.EditarProduto
			{
				BindingContext = p,
			});
		}
		catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK");
		}
	}

	private async void lst_produtos_Refreshing(object sender, EventArgs e)
	{
		try
		{
			lista.Clear();

			List<Produto> tmp = await App.Db.Getall();

			tmp.ForEach(i => lista.Add(i));
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
		finally
		{
			lst_produtos.IsRefreshing = false;
		}
	}

	private async Task ToolbarItem_Filtro(object sender, EventArgs e)
	{
		try
		{

		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
	}
	//Relatório

	private async void ToolbarItem_Clicked_2(object sender, EventArgs e)
	{
		var lista = await App.Db.Getall();

		var relatorio = lista
			.GroupBy(p => p.Categoria)
			.Select(g => new
			{
				Categoria = g.Key,
				Total = g.Sum(p => p.Total)
			})
			.OrderByDescending(x => x.Total);

		string msg = "";

		foreach (var item in relatorio)
		{
			msg += $"{item.Categoria}: R$ {item.Total:F2}\n";
		}

		await DisplayAlert("Relatório por Categoria", msg, "OK");
	}

	private async void OnFiltroChanged(object sender, EventArgs e)
	{
		try
		{
			string categoria = pickerFiltro.SelectedItem.ToString();

			lista.Clear();

			List<Produto> produtos = await App.Db.Getall();

			if (categoria != "Todos")
			{
				produtos = produtos
					.Where(p => p.Categoria == categoria)
					.ToList();
			}

			produtos.ForEach(p => lista.Add(p));
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", ex.Message, "OK");
		}
	}
}