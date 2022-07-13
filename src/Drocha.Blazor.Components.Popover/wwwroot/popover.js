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
export function calculateTopOffset(sourceRect, targetRect) {
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
export function calculateLeftOffset(sourceRect, targetRect) {
    const { width } = sourceRect;
    const { left, width: tWidth } = targetRect;
    const remainingWidth = window.innerWidth - left - width;
    let offset = left;
    if (remainingWidth <= 0) {
        offset = left - Math.abs(width - tWidth);
    }
    return offset;
}
export function updatePosition(source, target) {
    const sourceRect = source.getBoundingClientRect();
    const targetRect = target.getBoundingClientRect();
    source.style.top = `${calculateTopOffset(sourceRect, targetRect)}px`;
    source.style.left = `${calculateLeftOffset(sourceRect, targetRect)}px`;
}
//# sourceMappingURL=popover.js.map