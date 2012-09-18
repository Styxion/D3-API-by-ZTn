﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using ZTn.BNet.BattleNet;
using ZTn.BNet.D3.Careers;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;
using ZTn.BNet.D3.Artisans;

namespace ZTn.BNet.D3ProfileExplorer
{
    public partial class guiD3ProfileExplorer : Form
    {
        public guiD3ProfileExplorer()
        {
            InitializeComponent();

            List<Host> hosts = new List<Host>
            {
                new Host("Europe", "eu.battle.net"),
                new Host("America","us.battle.net"),
                new Host("Korea", "kr.battle.net"),
                new Host("Taiwan","tw.battle.net")
            };
            guiBattleNetHostList.DataSource = hosts;
            guiBattleNetHostList.DisplayMember = "name";

            List<Language> langs = new List<Language>
            {
                new Language("English", "en"),
                new Language("French","fr"),
                new Language("German", "de"),
                new Language("Italian","it"),
                new Language("Spanish", "es"),
                new Language("Polish","pl"),
                new Language("Portuguese","pt"),
                new Language("Russian","ru"),
                new Language("Turkish","tr"),
                new Language("Korean","ko"),
                new Language("Chinese","zh")
            };
            guiBattleNetLanguageList.DataSource = langs;
            guiBattleNetLanguageList.DisplayMember = "name";

            guiD3ProfileExplorerDllName.Text = Assembly.GetExecutingAssembly().GetName().Name;
            guiD3ProfileExplorerVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            guiBattleNetDllName.Text = typeof(BattleNet.BattleTag).Assembly.GetName().Name;
            guiBattleNetVersion.Text = typeof(BattleNet.BattleTag).Assembly.GetName().Version.ToString();
            guiD3APIDllName.Text = typeof(D3.D3Api).Assembly.GetName().Name;
            guiD3APIVersion.Text = typeof(D3.D3Api).Assembly.GetName().Version.ToString();
        }

        private void guiProfileLookup_Click(object sender, EventArgs e)
        {
            BattleTag battleTag = new BattleTag(guiBattleTag.Text);

            TreeNode node = new TreeNode("Career of " + battleTag.ToString());

            Career career = Career.getCareerFromBattleTag(battleTag);

            node.Nodes.AddRange(createNodeFromD3Object(career).ToArray());

            guiD3ProfileTreeView.Nodes.Add(node);
        }

        private List<TreeNode> createNodeFromD3Object(Object d3Object)
        {
            List<TreeNode> newNodes = new List<TreeNode>();

            Type type = d3Object.GetType();

            if (type.IsArray)
            {
                Object[] array = (Object[])d3Object;
                foreach (Object o in array)
                {
                    TreeNode newNode = new TreeNode(String.Format("[{0}]", o.GetType().Name));
                    newNode.Nodes.AddRange(createNodeFromD3Object(o).ToArray());
                    insertContextMenu(newNode, o);
                    newNodes.Add(newNode);
                }
            }
            else
            {
                if (type.FullName.Contains("ZTn.BNet.D3"))
                {
                    FieldInfo[] fieldInfos = type.GetFields();

                    foreach (FieldInfo fieldInfo in fieldInfos)
                    {
                        if (fieldInfo.GetValue(d3Object) != null)
                        {
                            TreeNode newNode = new TreeNode(fieldInfo.Name);
                            newNode.Nodes.AddRange(createNodeFromD3Object(fieldInfo.GetValue(d3Object)).ToArray());
                            insertContextMenu(newNode, fieldInfo.GetValue(d3Object));
                            newNodes.Add(newNode);
                        }
                    }
                }
                else
                {
                    TreeNode newNode = new TreeNode(d3Object.ToString());
                    newNodes.Add(newNode);
                }
            }
            return newNodes;
        }

        private void insertContextMenu(TreeNode node, Object d3Object)
        {
            node.Tag = d3Object;
            if (d3Object is HeroSummary)
            {
                node.ContextMenuStrip = guiHeroSummaryContextMenu;
                node.NodeFont = new Font(guiD3ProfileTreeView.Font, FontStyle.Underline);
            }
            else if (d3Object is ItemSummary)
            {
                node.ContextMenuStrip = guiItemSummaryContextMenu;
                node.NodeFont = new Font(guiD3ProfileTreeView.Font, FontStyle.Underline);
            }
            else if (d3Object is CareerArtisan)
            {
                node.ContextMenuStrip = guiCareerArtisanContextMenu;
                node.NodeFont = new Font(guiD3ProfileTreeView.Font, FontStyle.Underline);
            }
        }

        private void exploreHeroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HeroSummary heroSummary = (HeroSummary)guiD3ProfileTreeView.SelectedNode.Tag;

            BattleTag battleTag = new BattleTag(guiBattleTag.Text);

            TreeNode node = new TreeNode("Hero " + battleTag.ToString() + " / " + heroSummary.id);

            Hero hero = Hero.getHeroFromHeroId(battleTag, heroSummary.id);

            node.Nodes.AddRange(createNodeFromD3Object(hero).ToArray());

            guiD3ProfileTreeView.Nodes.Add(node);
        }

        private void guiD3ProfileTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                guiD3ProfileTreeView.SelectedNode = e.Node;
        }

        private void exploreItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemSummary itemSummary = (ItemSummary)guiD3ProfileTreeView.SelectedNode.Tag;

            TreeNode node = new TreeNode("Item " + itemSummary.tooltipParams);

            Item item = Item.getItemFromTooltipParams(itemSummary.tooltipParams);

            node.Nodes.AddRange(createNodeFromD3Object(item).ToArray());

            guiD3ProfileTreeView.Nodes.Add(node);
        }

        private void exploreICareerArtisanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CareerArtisan careerArtisan = (CareerArtisan)guiD3ProfileTreeView.SelectedNode.Tag;

            TreeNode node = new TreeNode("Artisan " + careerArtisan.slug);

            Artisan artisan = Artisan.getArtisanFromSlug(careerArtisan.slug);

            node.Nodes.AddRange(createNodeFromD3Object(artisan).ToArray());

            guiD3ProfileTreeView.Nodes.Add(node);
        }

        private void guiBattleNetHostList_TextChanged(object sender, EventArgs e)
        {
            D3.D3Api.host = ((Host)guiBattleNetHostList.SelectedItem).url;
        }

        private void guiBattleNetLanguageList_TextChanged(object sender, EventArgs e)
        {
            D3.D3Api.locale = ((Language)guiBattleNetLanguageList.SelectedItem).code;
        }
    }
}
