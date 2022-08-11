export function initializeWindowResizeObserver(dotNetReference: any) {
  window.addEventListener("resize", async (_) => {
    // await dotNetReference.invokeMethodAsync("WindowResized");
  });
}

interface Position {
  top: number;
  left: number;
}

export function setPosition(el: HTMLElement, position: Position) {
  el.style.top = `${position.top}px`;
  el.style.left = `${position.left}px`;
  el.style.visibility = "visible";
}
