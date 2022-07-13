export function initializeWindowResizeObserver(dotNetReference: any) {
  window.addEventListener("resize", async (_) => {
    await dotNetReference.invokeMethodAsync("WindowResized");
  });
}

export function calculateTopOffset(sourceRect: DOMRect, targetRect: DOMRect) {
  const { height } = sourceRect;
  const { top, bottom } = targetRect;

  // TODO: set padding in the C# code
  let offset = top - height - 2;
  if (offset < 0) {
    // flip to bottom if there's not enough space
    offset = bottom + 2;
  }
  return offset;
}

export function calculateLeftOffset(sourceRect: DOMRect, targetRect: DOMRect) {
  const { width } = sourceRect;
  const { left, width: tWidth } = targetRect;

  const remainingWidth = window.innerWidth - left - width;

  let offset = left;
  if (remainingWidth <= 0) {
    offset = left - Math.abs(width - tWidth);
  }
  return offset;
}

export function updatePosition(source: HTMLElement, target: HTMLElement) {
  const sourceRect = source.getBoundingClientRect();
  const targetRect = target.getBoundingClientRect();

  source.style.top = `${calculateTopOffset(sourceRect, targetRect)}px`;
  source.style.left = `${calculateLeftOffset(sourceRect, targetRect)}px`;
}