var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
export function initializeWindowResizeObserver(dotNetReference) {
    window.addEventListener("resize", (_) => __awaiter(this, void 0, void 0, function* () {
        yield dotNetReference.invokeMethodAsync("WindowResized");
    }));
}
export function updatePosition(source) {
    const sourceRect = source.getBoundingClientRect();
    const { innerWidth, innerHeight } = window;
    let top = Math.abs(sourceRect.height - innerHeight) / 2;
    let left = Math.abs(sourceRect.width - innerWidth) / 2;
    if (left === 0 && innerWidth > 300) {
        left = 24;
        source.style.maxWidth = `${sourceRect.width - 48}px`;
    }
    source.style.top = `${top}px`;
    source.style.left = `${left}px`;
}
//# sourceMappingURL=dialog.js.map