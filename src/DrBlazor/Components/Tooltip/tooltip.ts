export function observeMouse(dotnetRef: any, el: HTMLElement) {
  el.addEventListener("mouseenter", async () =>
    await dotnetRef.invokeMethodAsync("MouseEnter")
  );
  el.addEventListener("mouseleave", async () =>
    await dotnetRef.invokeMethodAsync("MouseLeave")
  );
}
