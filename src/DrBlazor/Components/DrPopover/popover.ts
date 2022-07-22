export function initializeWindowResizeObserver(dotNetReference: any) {
  window.addEventListener("resize", async (_) => {
    await dotNetReference.invokeMethodAsync("WindowResized");
  });
}

interface Position {
  top: number;
  left: number;
  needFlip: boolean;
}

enum Direction {
  Top,
  Right,
  Left,
  Bottom,
}

function canPlaceOnTop(
  elRect: DOMRect,
  targetRect: DOMRect,
  margin: number,
  target: HTMLElement,
): Position {
  const position: Position = { top: 0, left: 0, needFlip: false };

  const { height, width } = elRect;
  const { top, left, width: tWidth } = targetRect;

  position.top = top - height - margin;
  position.left = Math.abs(left - (width - tWidth) / 2);

  if (position.top < 0) {
    position.needFlip = true;
  }
  return position;
}

function canPlaceOnRight(
  sourceRect: DOMRect,
  targetRect: DOMRect,
  margin: number
): Position {
  const position: Position = { top: 0, left: 0, needFlip: false };

  const { innerWidth } = window;
  const { height, width } = sourceRect;
  const { top: tTop, right: tRight, height: tHeight } = targetRect;

  const center = Math.abs(height - tHeight) / 2;

  position.top = tTop - center;
  position.left = tRight + margin;

  if (position.left + width + margin > innerWidth) {
    position.needFlip = true;
  }
  return position;
}

function canPlaceOnLeft(
  sourceRect: DOMRect,
  targetRect: DOMRect,
  margin: number
): Position {
  const position: Position = { top: 0, left: 0, needFlip: false };

  const { height, width } = sourceRect;
  const { top: tTop, left: tLeft, height: tHeight } = targetRect;

  const center = Math.abs(height - tHeight) / 2;

  position.top = tTop - center;
  position.left = tLeft - width - margin;

  if (position.left < 0) {
    position.needFlip = true;
  }

  return position;
}

function canPlaceOnBottom(
  sourceRect: DOMRect,
  targetRect: DOMRect,
  margin: number
): Position {
  const position: Position = { top: 0, left: 0, needFlip: false };
  const { innerHeight } = window;
  const { height, width } = sourceRect;
  const { bottom, left, width: tWidth } = targetRect;

  position.top = bottom + margin;
  position.left = Math.abs(left - (width - tWidth) / 2);

  if (position.top + height > innerHeight) {
    position.needFlip = true;
  }
  return position;
}

export function updatePosition(
  el: HTMLElement,
  target: HTMLElement,
  direction: Direction,
  flipToFit: boolean,
  margin: number
) {
  const elRect = el.getBoundingClientRect();
  const targetRect = target.getBoundingClientRect();

  let top = 0;
  let left = 0;

  let placement = "top";

  switch (direction) {
    case Direction.Top:
      {
        let position = canPlaceOnTop(elRect, targetRect, margin, target);
        if (position.needFlip) {
          let attemptBottom = canPlaceOnBottom(elRect, targetRect, margin);
          if (!attemptBottom.needFlip) {
            position = attemptBottom;
            placement = "bottom";
          }
        }
        top = position.top;
        left = position.left;
      }
      break;

    case Direction.Right:
      {
        let position = canPlaceOnRight(elRect, targetRect, margin);
        placement = "right";

        if (position.needFlip && flipToFit) {
          let attemptLeft = canPlaceOnLeft(elRect, targetRect, margin);
          if (!attemptLeft.needFlip) {
            // if left has enough space put it there
            position = attemptLeft;
            placement = "left";
          }
          // otherwise keep it in the same place
          // TODO: try put it on top
        }
        top = position.top;
        left = position.left;
      }
      break;

    case Direction.Left:
      {
        let position = canPlaceOnLeft(elRect, targetRect, margin);
        placement = "left";

        if (position.needFlip && flipToFit) {
          let attemptRight = canPlaceOnRight(elRect, targetRect, margin);
          if (!attemptRight.needFlip) {
            // if left has enough space put it there
            position = attemptRight;
            placement = "right";
          }
          // otherwise keep it in the same place
          // TODO: try put it on top
        }
        top = position.top;
        left = position.left;
      }
      break;

    case Direction.Bottom:
      {
        let position = canPlaceOnBottom(elRect, targetRect, margin);
        placement = "bottom";

        if (position.needFlip) {
          let attemptTop = canPlaceOnTop(elRect, targetRect, margin, target);
          if (!attemptTop.needFlip) {
            position = attemptTop;
            placement = "top";
          }
        }
        top = position.top;
        left = position.left;
      }
      break;

    default:
      throw new Error("unknown popover direction");
  }

  el.style.top = `${top}px`;
  el.style.left = `${left}px`;

  el.setAttribute("data-dr-popover-placement", placement);
}
