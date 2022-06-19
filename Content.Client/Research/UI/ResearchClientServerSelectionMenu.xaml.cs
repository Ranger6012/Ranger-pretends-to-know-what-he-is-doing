using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.IoC;
using Robust.Shared.Localization;

namespace Content.Client.Research.UI
{
    [GenerateTypedNameReferences]
    public sealed partial class ResearchClientServerSelectionMenu : DefaultWindow
    {
        private int _serverCount;
        private string[] _serverNames = System.Array.Empty<string>();
        private int[] _serverIds = System.Array.Empty<int>();
        private int _selectedServerId = -1;

        private ResearchClientBoundUserInterface Owner { get; }

        public ResearchClientServerSelectionMenu(ResearchClientBoundUserInterface owner)
        {
            RobustXamlLoader.Load(this);
            IoCManager.InjectDependencies(this);

            Owner = owner;

            Servers.OnItemSelected += OnItemSelected;
            Servers.OnItemDeselected += OnItemDeselected;
        }

        public void OnItemSelected(ItemList.ItemListSelectedEventArgs itemListSelectedEventArgs)
        {
            Owner.SelectServer(_serverIds[itemListSelectedEventArgs.ItemIndex]);
        }

        public void OnItemDeselected(ItemList.ItemListDeselectedEventArgs itemListDeselectedEventArgs)
        {
            Owner.DeselectServer();
        }

        public void Populate(int serverCount, string[] serverNames, int[] serverIds, int selectedServerId)
        {
            _serverCount = serverCount;
            _serverNames = serverNames;
            _serverIds = serverIds;
            _selectedServerId = selectedServerId;

            // Disable so we can select the new selected server without triggering a new sync request.
            Servers.OnItemSelected -= OnItemSelected;
            Servers.OnItemDeselected -= OnItemDeselected;

            Servers.Clear();
            for (var i = 0; i < _serverCount; i++)
            {
                var id = _serverIds[i];
                Servers.AddItem(Loc.GetString("research-client-server-selection-menu-server-entry-text", ("id", id), ("serverName", _serverNames[i])));
                if (id == _selectedServerId)
                {
                    Servers[i].Selected = true;
                }
            }

            Servers.OnItemSelected += OnItemSelected;
            Servers.OnItemDeselected += OnItemDeselected;
        }
    }
}