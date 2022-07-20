export function initializeWindowResizeObserver(dotNetReference: any) {
  window.addEventListener("resize", async (_) => {
    await dotNetReference.invokeMethodAsync("WindowResized");
  });
}

export function updatePosition(source: HTMLElement) {
  const sourceRect = source.getBoundingClientRect();
  const { innerWidth, innerHeight } = window;

  let top = Math.abs(sourceRect.height - innerHeight) / 2;
  let left = Math.abs(sourceRect.width - innerWidth) / 2;

  if (left === 0 && innerWidth > 300) {
    // add some margin
    left = 24;
    source.style.maxWidth = `${sourceRect.width - 48}px`;
  }

  // source.addEventListener("click", (ev) => ev.stopPropagation());

  source.style.top = `${top}px`;
  source.style.left = `${left}px`;
}
