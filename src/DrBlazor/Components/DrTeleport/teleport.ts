export function teleport(source: HTMLElement, to: string) {
  const target = document.querySelector(to);
  if (!target) {
    throw new Error(`teleport: ${to} is not found on the DOM`);
  }
  target.appendChild(source);
}

export function removeFromDOM(el: HTMLElement) {
  if (el && (el as any).__internalId !== null) el.remove();
}
