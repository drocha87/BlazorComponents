class Popover {
  initialize(root, target) {
    const provider = document.getElementById("drocha-popover-provider");
    if (!provider) {
      throw new Error("popover provider does not exist on DOM");
    }

    // move the root to the popover provider
    const rootParent = root.parent;
    rootParent.removeChild(root);
    provider.appendChild(root);

  }
}

const popover = new Popover();
