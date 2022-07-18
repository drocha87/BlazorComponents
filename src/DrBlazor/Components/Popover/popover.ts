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
  sourceRect: DOMRect,
  targetRect: DOMRect,
  margin: number
): Position {
  const position: Position = { top: 0, left: 0, needFlip: false };

  const { height, width } = sourceRect;
  const { top, left, width: tWidth } = targetRect;

  position.top = top - height - margin;
  position.left = Math.abs(left - ((width - tWidth) / 2));

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
  position.left = Math.abs(left - ((width - tWidth) / 2));

  if (position.top + height > innerHeight) {
    position.needFlip = true;
  }
  return position;
}

export function updatePosition(
  source: HTMLElement,
  target: HTMLElement,
  direction: Direction,
  flipToFit: boolean,
  margin: number
) {
  const sourceRect = source.getBoundingClientRect();
  const targetRect = target.getBoundingClientRect();

  let top = 0;
  let left = 0;

  let placement = "top";

  switch (direction) {
    case Direction.Top:
      {
        let position = canPlaceOnTop(sourceRect, targetRect, margin);
        if (position.needFlip) {
          let attemptBottom = canPlaceOnBottom(sourceRect, targetRect, margin);
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
        let position = canPlaceOnRight(sourceRect, targetRect, margin);
        placement = "right";

        if (position.needFlip && flipToFit) {
          let attemptLeft = canPlaceOnLeft(sourceRect, targetRect, margin);
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
        let position = canPlaceOnLeft(sourceRect, targetRect, margin);
        placement = "left";

        if (position.needFlip && flipToFit) {
          let attemptRight = canPlaceOnRight(sourceRect, targetRect, margin);
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
        let position = canPlaceOnBottom(sourceRect, targetRect, margin);
        placement = "bottom";

        if (position.needFlip) {
          let attemptTop = canPlaceOnTop(sourceRect, targetRect, margin);
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

  source.style.top = `${top}px`;
  source.style.left = `${left}px`;

  source.setAttribute("data-drocha-popover-placement", placement);
}
