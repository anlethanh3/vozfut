import { Button, Modal, FloatingLabel, Form } from 'react-bootstrap';
import { useEffect, useState } from 'react';
import { GoalProps } from '../slices/matchDetailSlice';

export default function Goals(props: { show: boolean, initial: GoalProps | undefined, onSubmit: (data: GoalProps) => void, onClose: () => void }) {
    const { show, onSubmit, onClose, initial } = props
    const [goal, setGoal] = useState<GoalProps>({ matchDetailId: 0, goal: 0, assist: 0 })
    const onValid = () => {
        onSubmit(goal)
    }

    useEffect(() => {
        if (initial) {
            setGoal(initial)
        }
    }, [initial])

    return (<>
        <Modal show={show} onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>Goal & Assist</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="goal.goal">
                        <FloatingLabel controlId="floatinggoal" label="Goal">
                            <Form.Control type="number" min={0} placeholder="Input Goal" value={goal.goal} onChange={(e) => setGoal({ ...goal, goal: parseInt(e.target.value) })} />
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="goal.assist">
                        <FloatingLabel controlId="floatinggoal" label="Assist">
                            <Form.Control type="number" min={0} placeholder="Input Assist" value={goal.assist} onChange={(e) => setGoal({ ...goal, assist: parseInt(e.target.value) })} />
                        </FloatingLabel>
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onClose}>
                    Close
                </Button>
                <Button variant="primary" onClick={onValid}>
                    Update
                </Button>
            </Modal.Footer>
        </Modal>
    </>)
}