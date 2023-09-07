import { useState } from "react"
import { Button, Accordion, Form, FormGroup, FloatingLabel, OverlayTrigger, Tooltip } from "react-bootstrap";
import { FaRemoveFormat, FaSearch } from "react-icons/fa";

const SearchMember = (props: { onSearchChanged: (props: { name: string }) => void, onSubmit: (props: { name: string }) => void }) => {
    const { onSearchChanged, onSubmit } = props

    const [name, setName] = useState('')

    const onChanged = (e: any) => {
        const value = e.target.value
        onSearchChanged({ name: name })
        setName(value)
    }

    const onSearch = () => {
        onSubmit({ name: name })
    }

    const onClear = () => {
        onSearchChanged({ name: '' })
        setName('')
    }

    return (
        <Accordion className="my-2" defaultActiveKey="0">
            <Accordion.Item eventKey="0">
                <Accordion.Header>Search box</Accordion.Header>
                <Accordion.Body>
                    <Form>
                        <FormGroup className="mb-3" controlId="addMember.Name">
                            <FloatingLabel controlId="floatingName" label="Name">
                                <Form.Control type="text" placeholder="Input name" value={name} onChange={onChanged} onBlur={onChanged} />
                            </FloatingLabel>
                        </FormGroup>
                    </Form>
                    <OverlayTrigger overlay={<Tooltip>Search</Tooltip>}>
                        <Button className="me-2" variant="primary" onClick={onSearch}><FaSearch /></Button>
                    </OverlayTrigger>
                    <OverlayTrigger overlay={<Tooltip>Clear</Tooltip>}>
                        <Button variant="secondary" onClick={onClear}><FaRemoveFormat /></Button>
                    </OverlayTrigger>
                </Accordion.Body>
            </Accordion.Item>
        </Accordion>
    )
}

export default SearchMember