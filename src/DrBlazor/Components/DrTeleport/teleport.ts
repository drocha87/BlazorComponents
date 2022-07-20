export function teleport(source: HTMLElement, to: string)
{
    const target = document.querySelector(to);
    if (!target) {
        throw new Error(`teleport: ${to} is not found on the DOM`);
    }
    target.appendChild(source);
}