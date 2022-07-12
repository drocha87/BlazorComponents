class Popover {
  element: HTMLElement | null = null;

  provider!: HTMLElement;
  target!: HTMLElement;

  visible: boolean = false;

  initialize(id: string, source: HTMLElement, target: HTMLElement) {
    const provider = document.getElementById("drocha-popover-provider");
    if (!provider) {
      throw new Error("popover provider does not exist on DOM");
    }
    console.log(target)

    this.provider = provider;
    this.target = target;

    this.element = document.querySelector(`[data-drocha-popover-id=\"${id}\"]`);

    const config = { attributes: true, childList: false, subtree: false };

    const mutationObserver = new MutationObserver((list) => {
      for (const mutation of list) {
        if (
          mutation.type === "attributes" &&
          mutation.attributeName === "data-visible"
        ) {
          const visible = source.getAttribute("data-visible");
          if (visible === "true") {
            this.open();
          } else {
            this.close();
          }
        }
      }
    });
    mutationObserver.observe(source, config);

    // const resizeObserver = new ResizeObserver((entries) => {
    //   for (const _ of entries) {
    //     if (this.visible) {
    //       this.updatePosition();
    //     }
    //   }
    // });
    // resizeObserver.observe(target);

    window.addEventListener("resize", (ev) => {
      if (this.visible) {
        this.updatePosition();
      }
    });
  }

  padding: number = 2;

  calculateTopOffset(sourceRect: DOMRect, targetRect: DOMRect) {
    const { height } = sourceRect;
    const { top, bottom } = targetRect;

    let offset = top - height - this.padding;
    if (offset < 0) {
      // flip to bottom if there's not enough space
      offset = bottom + this.padding;
    }
    return offset;
  }

  calculateLeftOffset(sourceRect: DOMRect, targetRect: DOMRect) {
    const { width } = sourceRect;
    const { left, width: tWidth } = targetRect;

    const remainingWidth = window.innerWidth - left - width;

    let offset = left;
    if (remainingWidth <= 0) {
      offset = left - Math.abs(width - tWidth);
    }
    return offset;
  }

  updatePosition() {
    if (this.element) {
      const targetRect = this.target.getBoundingClientRect();
      const sourceRect = this.element.getBoundingClientRect();

      this.element.style.top = `${this.calculateTopOffset(
        sourceRect,
        targetRect
      )}px`;
      this.element.style.left = `${this.calculateLeftOffset(
        sourceRect,
        targetRect
      )}px`;
    }
  }

  open() {
    if (this.element) {
      const providerDisplay = window.getComputedStyle(this.provider);
      if (providerDisplay.getPropertyValue("display") === "none") {
        this.provider.style.display = "block";
      }
      this.element.style.display = "block";
      this.updatePosition();
      this.visible = true;
    }
  }

  close() {
    const els = this.provider.querySelectorAll("[data-visible=true]");
    if (els.length <= 0) {
      this.provider.style.display = "none";
    }
    this.element!.style.display = "none";
    this.visible = false;
  }
}

export const popover = new Popover();
