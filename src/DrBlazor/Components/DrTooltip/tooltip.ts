export function observeMouse(dotnetRef: any, el: HTMLElement) {
  if (el) {
    el.addEventListener(
      "mouseenter",
      async () => {
        await dotnetRef.invokeMethodAsync("MouseEnter", el.getBoundingClientRect())
      }
    );
    el.addEventListener(
      "mouseleave",
      async () => await dotnetRef.invokeMethodAsync("MouseLeave")
    );
  }
}
