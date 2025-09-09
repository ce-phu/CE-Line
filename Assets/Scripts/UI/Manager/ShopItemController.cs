using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class ShopItemController : MonoBehaviour
{
	[SerializeField] private ShopItemTypes itemTypes;
	[SerializeField] private Button buyButton;
	[SerializeField] private TextMeshProUGUI priceText;
	[SerializeField] private GameObject disablePanel;
	[SerializeField] private TextMeshProUGUI quantityText;
	[SerializeField] private GameObject[] itemIcon;
	[SerializeField] private GameObject[] itemIconGrey;
	[SerializeField] private GameObject coinImage;
	[SerializeField] private GameObject coinImageGrey;

	private void OnEnable()
	{
		SetupValue();
		SetupEnable();
	}

	private void SetupValue()
	{
		switch (itemTypes)
		{
			case ShopItemTypes.TIMESTOP:
				priceText.text = Player.SHOP_TIMESTOP_PRICE.ToString();
				break;
			case ShopItemTypes.BOMB:
				priceText.text = Player.SHOP_BOMB_PRICE.ToString();
				break;
			case ShopItemTypes.HAMMER:
				priceText.text = Player.SHOP_HAMMER_PRICE.ToString();
				break;
			case ShopItemTypes.BUNDLE:
				priceText.text = Player.SHOP_BUNDLE_PRICE.ToString();
				break;
			default:
				break;
		}
	}

	public void SetupEnable()
	{
		foreach (var item in itemIcon)
		{
			item.SetActive(false);
		}

		foreach (var item in itemIconGrey)
		{
			item.SetActive(false);
		}
		
		switch (itemTypes)
		{
			case ShopItemTypes.TIMESTOP:
			{
				quantityText.text = "x" + Player.SHOP_TIMESTOP_QUANTITY.ToString();
				if (!Player.Instance.HasEnoughGold(Player.SHOP_TIMESTOP_PRICE))
				{
					disablePanel.SetActive(true);
					priceText.color = Color.red;
					
					itemIconGrey[0].SetActive(true);
					itemIconGrey[1].SetActive(true);
					coinImage.SetActive(false);
					coinImageGrey.SetActive(true);
				}
				else
				{
					disablePanel.SetActive(false);
					priceText.color = Color.white;
					buyButton.onClick.AddListener(OnClick_BuyButton);
					
					itemIcon[0].SetActive(true);
					itemIcon[1].SetActive(true);
					coinImage.SetActive(true);
					coinImageGrey.SetActive(false);
				}
				
				break;
			}
			case ShopItemTypes.BOMB:
			{
				quantityText.text = "x" + Player.SHOP_BOMB_QUANTITY.ToString();
				if (!Player.Instance.HasEnoughGold(Player.SHOP_BOMB_PRICE))
				{
					disablePanel.SetActive(true);
					priceText.color = Color.red;
					
					itemIconGrey[0].SetActive(true);
					itemIconGrey[2].SetActive(true);
					coinImage.SetActive(false);
					coinImageGrey.SetActive(true);
				}
				else
				{
					disablePanel.SetActive(false);
					priceText.color = Color.white;
					buyButton.onClick.AddListener(OnClick_BuyButton);
					
					itemIcon[0].SetActive(true);
					itemIcon[2].SetActive(true);
					coinImage.SetActive(true);
					coinImageGrey.SetActive(false);
				}
				
				break;
			}
			case ShopItemTypes.HAMMER:
			{
				quantityText.text = "x" + Player.SHOP_HAMMER_QUANTITY.ToString();
				if (!Player.Instance.HasEnoughGold(Player.SHOP_HAMMER_PRICE))
				{
					disablePanel.SetActive(true);
					priceText.color = Color.red;
					
					itemIconGrey[0].SetActive(true);
					itemIconGrey[3].SetActive(true);
					coinImage.SetActive(false);
					coinImageGrey.SetActive(true);
				}
				else
				{
					disablePanel.SetActive(false);
					priceText.color = Color.white;
					buyButton.onClick.AddListener(OnClick_BuyButton);
					
					itemIcon[0].SetActive(true);
					itemIcon[3].SetActive(true);
					coinImage.SetActive(true);
					coinImageGrey.SetActive(false);
				}
				
				break;
			}
			case ShopItemTypes.BUNDLE:	
			{				
				quantityText.text = "x" + Player.SHOP_BUNDLE_QUANTITY.ToString();
				if (!Player.Instance.HasEnoughGold(Player.SHOP_BUNDLE_PRICE))
				{
					disablePanel.SetActive(true);
					priceText.color = Color.red;
					
					itemIconGrey[0].SetActive(true);
					itemIconGrey[4].SetActive(true);
					coinImage.SetActive(false);
					coinImageGrey.SetActive(true);
				}
				else
				{
					disablePanel.SetActive(false);
					priceText.color = Color.white;
					buyButton.onClick.AddListener(OnClick_BuyButton);
					
					itemIcon[0].SetActive(true);
					itemIcon[4].SetActive(true);
					coinImage.SetActive(true);
					coinImageGrey.SetActive(false);
				}
				
				break;
			}
			default:
				break;
		}
	}
	
	private void OnClick_BuyButton()
	{
		if (SystemManager.excludeButton) return;
		SystemManager.excludeButton = true;        
        
		VibrationManager.VibrateTap();
		SoundManager.PlaySE( SE.UI_BTNCLICK );
		SoundManager.PlaySE(SE.SHOP_PURCHASED);
		
		switch (itemTypes)
		{
			case ShopItemTypes.TIMESTOP:
			{
				if (!Player.Instance.HasEnoughGold(Player.SHOP_TIMESTOP_PRICE)) return;
				
				//Add item
				Player.Instance.AddPowerup(PowerupTypes.TIMESTOP, Player.SHOP_TIMESTOP_QUANTITY);
				Player.Instance.UseGold(Player.SHOP_TIMESTOP_PRICE);
				ItemPopupUI.In(ItemType.TIMESTOP, Player.SHOP_TIMESTOP_QUANTITY);
				
				break;
			}
			case ShopItemTypes.BOMB:
			{
				if (!Player.Instance.HasEnoughGold(Player.SHOP_BOMB_PRICE)) return;
				
				//Add item
				Player.Instance.AddPowerup(PowerupTypes.BOMB, Player.SHOP_BOMB_QUANTITY);
				Player.Instance.UseGold(Player.SHOP_BOMB_PRICE);
				ItemPopupUI.In(ItemType.BOMB, Player.SHOP_BOMB_QUANTITY);
				
				break;
			}
			case ShopItemTypes.HAMMER:
			{
				if (!Player.Instance.HasEnoughGold(Player.SHOP_HAMMER_PRICE)) return;
				
				//Add item
				Player.Instance.AddPowerup(PowerupTypes.HAMMER, Player.SHOP_HAMMER_QUANTITY);
				Player.Instance.UseGold(Player.SHOP_HAMMER_PRICE);
				ItemPopupUI.In(ItemType.HAMMER, Player.SHOP_HAMMER_QUANTITY);
				
				break;
			}
			case ShopItemTypes.BUNDLE:
			{
				if (!Player.Instance.HasEnoughGold(Player.SHOP_BUNDLE_PRICE)) return;
				//Add item
				Player.Instance.AddPowerup(PowerupTypes.TIMESTOP, Player.SHOP_BUNDLE_QUANTITY);
				Player.Instance.AddPowerup(PowerupTypes.BOMB, Player.SHOP_BUNDLE_QUANTITY);
				Player.Instance.AddPowerup(PowerupTypes.HAMMER, Player.SHOP_BUNDLE_QUANTITY);
				
				Player.Instance.UseGold(Player.SHOP_BUNDLE_PRICE);
				ItemPopupUI.In(ItemType.BUNDLE, Player.SHOP_BUNDLE_QUANTITY);
				
				break;
			}
			default:
				break;
		}
		
		ShopUIManager.UpdateShopItem();
		SystemManager.excludeButton = false;
	}
	
	private void OnDisable()
	{
		buyButton.onClick.RemoveAllListeners();
	}
}
